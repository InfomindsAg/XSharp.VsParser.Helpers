using LanguageService.SyntaxTree;
using LanguageService.SyntaxTree.Tree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace XSharp.VsParser.Helpers.Parser
{
    public class AbstractSyntaxTree : IEnumerable<IParseTree>
    {
        readonly ParserHelper _ParserHelper;
        TokenStreamRewriter _TokenStreamRewriter = null;

        /// <summary>
        /// Get's a Rewriter that can be used to modify the SourceTree
        /// </summary>
        public TokenStreamRewriter Rewriter
        {
            get => _TokenStreamRewriter ??= new TokenStreamRewriter(_ParserHelper._Tokens);
        }

        public void Clear()
        {
            _TokenStreamRewriter = null;
        }

        internal AbstractSyntaxTree(ParserHelper parserHelper)
        {
            _ParserHelper = parserHelper;
        }

        IEnumerator<IParseTree> GetEnumerator(IParseTree start)
        {
            yield return start;
            for (int i = 0; i < start.ChildCount; i++)
            {
                var childEnumerator = GetEnumerator(start.GetChild(i));
                while (childEnumerator.MoveNext())
                    yield return childEnumerator.Current;
            }
        }

        public IEnumerator<IParseTree> GetEnumerator()
        {
            _ParserHelper.CheckParseSuccessful();
            return GetEnumerator(_ParserHelper._StartRule);
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
            _ParserHelper.CheckParseSuccessful();
            return _ParserHelper._StartRule.DumpYaml();
        }

        /// <summary>
        /// Dumps the AST created by parsing as XDocument
        /// </summary>
        public XDocument DumpXml()
        {
            _ParserHelper.CheckParseSuccessful();
            return _ParserHelper._StartRule.DumpXml();
        }

    }
}
