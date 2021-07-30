using LanguageService.CodeAnalysis.XSharp.SyntaxParser;
using LanguageService.SyntaxTree;
using LanguageService.SyntaxTree.Tree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using XSharp.VsParser.Helpers.Rewriter;

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

        bool SaveRewriteResult(string newFileName, bool createBackup = false)
        {
            if (_TokenStreamRewriter == null)
                return false;

            var newSourceCode = _TokenStreamRewriter.GetText();
            if (_SourceCode == newSourceCode)
                return false;

            if (createBackup)
            {
                var backupName = Path.ChangeExtension(FileName, ".BeforeRewrite");
                if (File.Exists(backupName))
                    File.Delete(backupName);
                File.Move(FileName, backupName);
            }

            File.WriteAllText(newFileName, newSourceCode);
            return true;
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

        public RewriterForContext<T> RewriterFor<T>(T context) where T : IParseTree
            => new RewriterForContext<T>(Rewriter, context);

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

        /// <summary>
        /// Saves the code as file with the specified filename, if it was changed trougth rewrites
        /// </summary>
        /// <param name="newFileName">The new filename for the file</param>
        /// <returns>True, if the file was changed and therefor saved, otherwise false.</returns>
        public bool SaveRewriteResult(string newFileName)
            => SaveRewriteResult(newFileName, false);

        /// <summary>
        /// Saves the code using original filename, if it was changed trougth rewrites. 
        /// </summary>
        /// <param name="createBackup">If true, a backup will be created before save the file. The file extension for the backup will be beforeRewrite</param>
        /// <returns>True, if the file was changed and therefor saved, otherwise false.</returns>
        public bool SaveRewriteResult(bool createBackup = false)
            => SaveRewriteResult(FileName, createBackup);


        public void ExecuteListeners(List<XSharpBaseListener> listeners)
        {
            Debug.Assert((listeners?.Count ?? 0) > 0, "List of listeners can not be empty");
            CheckParseSuccessful();

            foreach (var listener in listeners)
                ParseTreeWalker.Default.Walk(listener, _StartRule);
        }

    }
}
