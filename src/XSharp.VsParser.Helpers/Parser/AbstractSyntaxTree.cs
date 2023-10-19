using LanguageService.CodeAnalysis.XSharp.SyntaxParser;
using LanguageService.SyntaxTree;
using LanguageService.SyntaxTree.Tree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Linq;
using XSharp.VsParser.Helpers.Rewriter;

namespace XSharp.VsParser.Helpers.Parser
{
    /// <summary>
    /// The AbstractSyntaxTree class wraps the parsing result in a class, that provides additional features like 
    /// <list type="bullet">
    /// <item><description>Making it enumerable</description></item>
    /// <item><description>Dump it as Yaml or XML</description></item>
    /// <item><description>Creating Rewriters</description></item>
    /// <item><description>Saving the result of code rewrites</description></item>
    /// <item><description>Exectuing XSharpBaseListeners</description></item>
    /// </list>
    /// </summary>
    public class AbstractSyntaxTree : IEnumerable<IParseTree>
    {
        #region Private Fields

        TokenStreamRewriter _TokenStreamRewriter = null;
        readonly string _SourceCode;
        readonly ITokenStream _Tokens;
        readonly XSharpParserRuleContext _StartRule;

        #endregion

        #region Private Helper Methods

        Encoding GetFileEncoding(string fileName)
        {
            if (File.Exists(fileName))
            {
                using (var fileStream = new FileStream(fileName, FileMode.Open))
                {
                    if (fileStream.Length > 3)
                    {
                        var bits = new byte[3];
                        fileStream.Read(bits, 0, 3);

                        var utf8Bom = (bits[0] == 0xEF && bits[1] == 0xBB && bits[2] == 0xBF);
                        return new UTF8Encoding(utf8Bom);
                    }
                }
            }
            return Encoding.UTF8;
        }

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

            File.WriteAllText(newFileName, newSourceCode, GetFileEncoding(newFileName));
            ResetRewriter();
            return true;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The FileNane of the Prg File, that was parsed to generate the
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Get's a Rewriter that can be used to modify the SourceTree
        /// </summary>
        public TokenStreamRewriter Rewriter
        {
            get
            {
                if (_TokenStreamRewriter == null)
                {
                    _TokenStreamRewriter = new TokenStreamRewriter(_Tokens);
                    var trimChars = new char[] { '\uFEFF', '\u200B', ' ', '\r', '\n' };
                    // Ensure that the rewriter doesn't change break code. (Example " "" " => " " "
                    var emptyRewriteResult = _TokenStreamRewriter.GetText();
                    if (_SourceCode.Trim(trimChars) != emptyRewriteResult.Trim(trimChars))
                        throw new RewriterException(_SourceCode, emptyRewriteResult);
                }

                return _TokenStreamRewriter;
            }
        }

        #endregion

        internal AbstractSyntaxTree(string fileName, string sourceCode, ITokenStream tokens, XSharpParserRuleContext startRule)
        {
            _SourceCode = sourceCode;
            _Tokens = tokens;
            _StartRule = startRule;
            FileName = fileName;
        }

        /// <summary>
        /// Clearst the AbstractSyntaxTree
        /// </summary>
        public void Clear()
        {
            _TokenStreamRewriter = null;
        }

        /// <summary>
        /// Returns an Enumerator for the AbstractSyntaxTree
        /// </summary>
        /// <returns></returns>
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
        /// Returns a Rewriter for a specified context
        /// </summary>
        /// <typeparam name="T">The type of the rewriter conext (ex. MethodContext, Class_Context, ...)</typeparam>
        /// <param name="context">The context</param>
        /// <returns>A RewriterForContext object</returns>
        public RewriterForContext<T> RewriterFor<T>(T context) where T : IParseTree
            => new(Rewriter, context);

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

        /// <summary>
        /// Returns the code changed trougth rewrites. 
        /// </summary>
        /// <returns>The rewritten code</returns>
        public string GetRewriteResult()
            => _TokenStreamRewriter != null ? _TokenStreamRewriter.GetText() : _SourceCode;

        /// <summary>
        /// Resets the rewriter
        /// </summary>
        public void ResetRewriter()
            => _TokenStreamRewriter = null;

        /// <summary>
        /// Executes a list of XSharpBaseListener instances on the AbstractSyntaxTree
        /// </summary>
        /// <param name="listeners">A list of XSharpBaseListener instances</param>
        public void ExecuteListeners(List<XSharpBaseListener> listeners)
        {
            Debug.Assert((listeners?.Count ?? 0) > 0, "List of listeners can not be empty");
            CheckParseSuccessful();

            foreach (var listener in listeners)
                ParseTreeWalker.Default.Walk(listener, _StartRule);
        }

    }
}
