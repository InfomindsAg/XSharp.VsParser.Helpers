using LanguageService.CodeAnalysis.XSharp.SyntaxParser;
using LanguageService.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.Parser.Helpers.Listeners;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace IM.DevTools.XsFormToWinForm.Parser.Rewriters
{
	public class XSharpBaseRewriter : ExtendedXSharpBaseListener
	{
		protected TokenStreamRewriter _Rewriter = null;
		public List<string> Warnings = new();


        public void Initialize(TokenStreamRewriter rewriter)
        {
            _Rewriter = rewriter;
            Warnings.Clear();
        }


        #region Private methods

        int GetIndex(IToken token)
        {
            if (token is XSharpToken xsToken && xsToken != null)
                return xsToken.OriginalTokenIndex;

            throw new ArgumentException("Token must be a XSharpToken and can not be null");
        }

        int GetIndex(IdentifierContext id)
            => GetIndex(id?.Token);

        #endregion

        #region Rewriting Methods

        protected void ChangeIdentifier(IdentifierContext id, string newId)
        {
            _Rewriter.Replace(GetIndex(id), newId);
        }

        protected void RemoveParameters(ParameterListContext parameterList)
        {
            if (parameterList == null || (parameterList._Params?.Count ?? 0) == 0)
                return;

            _Rewriter.Replace(GetIndex(parameterList.Start), GetIndex(parameterList.Stop), "()");
        }

        protected void ChangeMethodToVoidStrict(SignatureContext signature)
        {
            // _Rewriter.Replace(GetIndex(id), newId);
            var addVoid = signature.Type == null;
            var addStrict = signature.CallingConvention == null;
            if (!addVoid && !("void".Equals(signature.Type.GetText(), StringComparison.CurrentCultureIgnoreCase)))
                _Rewriter.Replace(GetIndex(signature.Type.Start), "void");
            if (!addStrict && !("strict".Equals(signature.CallingConvention.GetText(), StringComparison.CurrentCultureIgnoreCase)))
                _Rewriter.Replace(GetIndex(signature.CallingConvention.Start), "strict");

            if (addVoid || addStrict)
            {
                var text = "";
                if (addVoid)
                    text += " as void";
                if (addStrict)
                    text += " strict";

                _Rewriter.InsertAfter(GetIndex(signature.parameterList().Stop), text);
            }
        }
        #endregion

    }
}
