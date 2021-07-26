using LanguageService.CodeAnalysis.XSharp.SyntaxParser;
using LanguageService.SyntaxTree;
using LanguageService.SyntaxTree.Tree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;

namespace XSharp.VsParser.Helpers.Parser
{
    public class AbstractSyntaxTree : IEnumerable<IParseTree>
    {
        #region Private Fields

        TokenStreamRewriter _TokenStreamRewriter = null;
        readonly string _SourceCode;
        readonly ITokenStream _Tokens;
        readonly XSharpParserRuleContext _StartRule;

        #endregion

        #region Private Helper Methods

        void CheckParseSuccessful()
        {
            if (_StartRule == null)
                throw new ArgumentException("Parsing was not successful");
        }

        #endregion

        #region Public Properties

        public string FileName { get; private set; }

        /// <summary>
        /// Get's a Rewriter that can be used to modify the SourceTree
        /// </summary>
        public TokenStreamRewriter Rewriter
        {
            get => _TokenStreamRewriter ??= new TokenStreamRewriter(_Tokens);
        }

        #endregion

        internal AbstractSyntaxTree(string fileName, string sourceCode, ITokenStream tokens, XSharpParserRuleContext startRule)
        {
            _SourceCode = sourceCode;
            _Tokens = tokens;
            _StartRule = startRule;
            FileName = fileName;
        }

        public void Clear()
        {
            _TokenStreamRewriter = null;
        }

        public IEnumerator<IParseTree> GetEnumerator()
        {
            CheckParseSuccessful();
            return new ParseTreeEnumerable(_StartRule).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Dumps the AST created by parsing as Yaml
        /// </summary>
        public string DumpYaml()
        {
            CheckParseSuccessful();
            return _StartRule.DumpYaml();
        }

        /// <summary>
        /// Dumps the AST created by parsing as XDocument
        /// </summary>
        public XDocument DumpXml()
        {
            CheckParseSuccessful();
            return _StartRule.DumpXml();
        }

        public void SaveRewriteResult()
            => SaveRewriteResult(FileName);

        public void SaveRewriteResult(string newFileName)
        {
            if (_TokenStreamRewriter == null)
                return;

            var newSourceCode = _TokenStreamRewriter.GetText();
            if (_SourceCode == newSourceCode)
                return;

            File.WriteAllText(newFileName, newSourceCode);
        }

        public void ExecuteListeners(List<XSharpBaseListener> listeners)
        {
            Debug.Assert((listeners?.Count ?? 0) > 0, "List of listeners can not be empty");
            CheckParseSuccessful();

            foreach (var listener in listeners)
                ParseTreeWalker.Default.Walk(listener, _StartRule);
        }

    }
}
