using LanguageService.SyntaxTree;
using LanguageService.SyntaxTree.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Rewriter
{
    static class InternalRewriterHelper
    {

        public static void ReplaceCallingConvention(TokenStreamRewriter rewriter, string newCallingConvention, CallingconventionContext callingConvention, int insertAfterTokenIndex)
        {
            if (string.IsNullOrEmpty(newCallingConvention))
                throw new ArgumentException($"{nameof(newCallingConvention)} can not be empty");

            if (callingConvention != null)
                rewriter.Replace(callingConvention.Convention.ToIndex(), newCallingConvention);
            else
                rewriter.InsertAfter(insertAfterTokenIndex, " " + newCallingConvention);
        }

        public static void DeleteCallingConvention(TokenStreamRewriter rewriter, CallingconventionContext callingConvention)
        {
            if (callingConvention != null)
                rewriter.Delete(callingConvention.Convention.ToIndex());
        }


        public static void DeleteAllParameters(TokenStreamRewriter rewriter, ParameterListContext paramList)
        {
            if ((paramList?._Params?.Count ?? 0) > 0)
                rewriter.Replace(paramList.Start.ToIndex(), paramList.Stop.ToIndex(), "()");
        }

        public static void ReplaceParameters(TokenStreamRewriter rewriter, string newParameters, ParameterListContext paramList, int insertAfterTokenIndex)
        {
            newParameters = $"({newParameters})";

            if (paramList != null)
                rewriter.Replace(paramList.Start.ToIndex(), paramList.Stop.ToIndex(), newParameters);
            else
                rewriter.InsertAfter(insertAfterTokenIndex, newParameters);
        }

        public static string RemoveAsFromType(string type)
        {
            if (type.TrimStart().StartsWith("as ", StringComparison.OrdinalIgnoreCase))
                return type.TrimStart().Substring(3).TrimStart();

            return type;
        }

        public static string AddAsToType(string type)
        {
            if (!type.TrimStart().StartsWith("as ", StringComparison.OrdinalIgnoreCase))
                return $"as {type}";
            return type;
        }
    }
}
