using LanguageService.CodeAnalysis.XSharp;
using LanguageService.CodeAnalysis.XSharp.SyntaxParser;
using LanguageService.SyntaxTree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace XSharp.VsParser.Helpers.Parser
{
    /// <summary>
    /// The ParserHelper class. Use the BuildWith... Static methods to instantiate the class
    /// </summary>
    public class ParserHelper
    {
        readonly GenericErrorListener _ErrorListener = new();
        readonly XSharpParseOptions _XSharpOptions;
        private BufferedTokenStream _XSharpTokenStream;

        /// <summary>
        /// The Abstract Syntax Tree. Will be initialized by parsing a file.
        /// </summary>
        public AbstractSyntaxTree Tree { get; private set; }

        /// <summary>
        /// Obsolete
        /// </summary>
        [Obsolete("Replaced by Tree Property")]
        public AbstractSyntaxTree SourceTree => Tree;

        /// <summary>
        /// The source code, that was parsed
        /// </summary>
        public string SourceCode { get; private set; }

        /// <summary>
        /// The source code lines, that was parsed 
        /// </summary>
        public string[] SourceCodeLines => SourceCode?.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

        /// <summary>
        /// A list of the comments. Will be initialized by parsing a file.
        /// </summary>
        public List<IToken> Comments => _XSharpTokenStream?.GetTokens().Where(q => XSharpLexer.IsComment(q.Type)).ToList();

        internal ParserHelper(XSharpParseOptions xsharpOptions)
        {
            XSharpSpecificCompilationOptions.SetDefaultIncludeDir(@"c:\Program Files(x86)\XSharp\Include\");
            XSharpSpecificCompilationOptions.SetWinDir(Environment.GetFolderPath(Environment.SpecialFolder.Windows));
            XSharpSpecificCompilationOptions.SetSysDir(Environment.GetFolderPath(Environment.SpecialFolder.System));

            _XSharpOptions = xsharpOptions;

            Tree = null;
        }

        /// <summary>
        /// Clears the ParserHelper
        /// </summary>
        public void Clear()
        {
            _ErrorListener.Clear();
            Tree = null;
            _XSharpTokenStream = null;
            SourceCode = null;
        }

        /// <summary>
        /// Loads and parses a file
        /// </summary>
        /// <param name="fileName">The fileName</param>
        /// <returns>A result instance</returns>
        public Result ParseFile(string fileName)
            => ParseText(File.ReadAllText(fileName), fileName);

        /// <summary>
        /// Parses the sourceCode 
        /// </summary>
        /// <param name="sourceCode">The sourceCode</param>
        /// <param name="fileName">The fileName of the file, that contains the sourceCode</param>
        /// <returns>A result instance</returns>
        public Result ParseText(string sourceCode, string fileName)
        {
            Clear();

            try
            {
                var ok = XSharp.Parser.VsParser.Parse(sourceCode, fileName, _XSharpOptions, _ErrorListener, out var tokens, out var startRule);
                if (!ok && _ErrorListener.Result.OK)
                    _ErrorListener.Result.Errors.Add(new Result.Item { Message = "Generic Parse Error", Line = 0 });
                if (_ErrorListener.Result.OK)
                {
                    Tree = new AbstractSyntaxTree(fileName, sourceCode, tokens, startRule);
                    _XSharpTokenStream = tokens as BufferedTokenStream;
                    SourceCode = sourceCode;
                }
            }
            catch (Exception ex)
            {
                _ErrorListener.Result.Errors.Add(new Result.Item { Message = "Exception: " + ex.Message, Line = 0 });
            }

            return _ErrorListener.Result;
        }

        /// <summary>
        /// Re-Parses the tree to include rewrites
        /// </summary>
        /// <returns></returns>
        public Result ParseRewriter()
            => ParseText(Tree.Rewriter.GetText(), Tree.FileName);

        #region Builders

        /// <summary>
        /// Creates a new instance of the ParserHelper, using the default Vo Options
        /// </summary>
        /// <returns>A new ParserHelper instance</returns>
        public static ParserHelper BuildWithVoDefaultOptions()
                 => new(XSharpParseOptions.FromVsValues(new List<string>
                    {
                    @"i:c:\Program Files(x86)\XSharp\Include\",
                    "dialect:VO",
                    "d:DEBUG;__XSHARP_RT__",
                    "az-",
                    "cs-",
                    "lb+",
                    "ovf-",
                    "unsafe+",
                    "ins+",
                    "ns-",
                    "vo1-",
                    "vo2+",
                    "vo3+",
                    "vo4-",
                    "vo5+",
                    "vo6+",
                    "vo7+",
                    "vo8+",
                    "vo9-",
                    "vo10+",
                    "vo11-",
                    "vo12-",
                    "vo13+",
                    "vo14-",
                    "vo15+",
                    "vo16-",
                    }));

        /// <summary>
        /// Creates a new instance of the ParserHelper, using the options in the list
        /// </summary>
        /// <returns>A new ParserHelper instance</returns>
        public static ParserHelper BuildWithOptionsList(List<string> options)
            => new(XSharpParseOptions.FromVsValues(options));

        #endregion
    }
}
