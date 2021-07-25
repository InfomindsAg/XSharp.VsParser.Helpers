using LanguageService.CodeAnalysis.XSharp.SyntaxParser;
using LanguageService.SyntaxTree;
using LanguageService.SyntaxTree.Tree;
using System;
using System.Collections.Generic;
using System.Text;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser
{
    public static class AbstractSyntaxTreeExtensions
    {
        public static IEnumerable<T> WhereType<T>(this IEnumerable<IParseTree> enumerable) where T : IParseTree
        {
            foreach (var item in enumerable)
                if (item is T returnItem)
                    yield return returnItem;
        }

        public static T FirstParentOrDefault<T>(this IParseTree element) where T : IParseTree
        {
            element = element?.Parent;
            while (element != null)
            {
                if (element is T returnElement)
                    return returnElement;
                element = element?.Parent;
            }

            return default(T);
        }
    }
}
