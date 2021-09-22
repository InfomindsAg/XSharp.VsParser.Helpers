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
        class LineInfo
        {
            public int Line { get; set; }
            public int Start { get; set; }
            public int End { get; set; }
        }

        readonly XSharpParseOptions _XSharpOptions;
        private BufferedTokenStream _XSharpTokenStream;
        private List<LineInfo> _Lines;

        List<LineInfo> BuildLineInfo()
        {
            if (string.IsNullOrEmpty(SourceCode))
                return null;

            var line = 1;
            var result = new List<LineInfo>() { };
            var start = 0;
            do
            {
                var end = SourceCode.IndexOf('\n', start);
                if (end == -1)
                    end = SourceCode.Length;
                result.Add(new LineInfo { Start = start, End = end, Line = line });
                line++;
                start = end + 1;
            } while (start < SourceCode.Length);

            // Additional line for the last newline
            result.Add(new LineInfo { Start = start, End = start, Line = line });

            return result;
        }

        string[] BuildSourceCodeLines()
        {
            if (string.IsNullOrEmpty(SourceCode))
                return new string[0];

            var result = SourceCode.Split(new string[] { "\n" }, StringSplitOptions.None);
            if (result.Length > 0 && result[0].EndsWith("\r"))
                result = result.Select(q => q.TrimEnd('\r')).ToArray();
            return result;
        }


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
        public string[] SourceCodeLines { get; private set; } 

        /// <summary>
        /// A list of the comments. Will be initialized by parsing a file.
        /// </summary>
        public IReadOnlyList<TokenValues> Comments { get; private set; }

        /// <summary>
        /// A list of the Tokens. Will be initialized by parsing a file.
        /// </summary>
        public IReadOnlyList<TokenValues> Tokens { get; private set; }


        internal ParserHelper(XSharpParseOptions xsharpOptions)
        {
            XSharpSpecificCompilationOptions.SetDefaultIncludeDir(@"c:\Program Files(x86)\XSharp\Include\");
            XSharpSpecificCompilationOptions.SetWinDir(Environment.GetFolderPath(Environment.SpecialFolder.Windows));
            XSharpSpecificCompilationOptions.SetSysDir(Environment.GetFolderPath(Environment.SpecialFolder.System));

            _XSharpOptions = xsharpOptions;

            Tree = null;
        }

        /// <summary>
        /// Calculates the line and column based on an absolute position in source code
        /// </summary>
        /// <param name="positionInSourceCode">The absolute position in source code</param>
        /// <returns>The line and column (1 based)</returns>
        public (int Line, int Column) GetLineAndColumn(int positionInSourceCode)
        {
            var line = _Lines.LastOrDefault(q => q.Start <= positionInSourceCode && positionInSourceCode <= q.End);
            if (line == null)
                throw new ArgumentException("Invalid positionInSourceCode");

            return (line.Line, positionInSourceCode - line.Start + 1);
        }

        /// <summary>
        /// Calculates the start and end line and column for a token
        /// </summary>
        /// <param name="token">The token</param>
        /// <returns>The start and end line and column (1 based)</returns>
        public (int StartLine, int StartColumn, int EndLine, int EndColumn) GetTokenPosition(IToken token)
        {
            var (startLine, startColumn) = GetLineAndColumn(token.StartIndex);
            var (endLine, endColumn) = GetLineAndColumn(token.StopIndex);
            return (startLine, startColumn, endLine, endColumn);
        }

        /// <summary>
        /// Clears the ParserHelper
        /// </summary>
        public void Clear()
        {
            Tree = null;
            _XSharpTokenStream = null;
            _Lines = null;
            SourceCode = null;
            SourceCodeLines = new string[0];
            Comments = null;
            Tokens = null;
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
            var errorListener = new GenericErrorListener();

            try
            {
                for (int retry = 0; retry < 3; retry++)
                {
                    errorListener.Clear();
                    var ok = XSharp.Parser.VsParser.Parse(sourceCode, fileName, _XSharpOptions, errorListener, out var tokens, out var startRule);
                    if (!ok && errorListener.Result.OK)
                        errorListener.Result.Errors.Add(new Result.Item { Message = "Generic Parse Error", Line = 0 });

                    // Workaround for known issue. Sometimes a "Include file not found" is found.
                    if (!errorListener.Result.OK && errorListener.Result.Errors.Any(q => q.Message.Contains("Include file not found")))
                        continue;

                    if (errorListener.Result.OK)
                    {
                        Tree = new AbstractSyntaxTree(fileName, sourceCode, tokens, startRule);
                        _XSharpTokenStream = tokens as BufferedTokenStream;
                        SourceCode = sourceCode;
                        SourceCodeLines = BuildSourceCodeLines();
                        _Lines = BuildLineInfo();
                        Tokens = _XSharpTokenStream?.GetTokens().Select(q => TokenValues.Build(q, this)).ToList();
                        Comments = Tokens.Where(q => q.Type == TokenType.Comment).ToList();
                    }
                    break;
                }
            }
            catch (Exception ex)
            {
                errorListener.Result.Errors.Add(new Result.Item { Message = "Exception: " + ex.Message, Line = 0 });
            }

            return errorListener.Result;
        }

        /// <summary>
        /// Re-Parses the tree to include rewrites
        /// </summary>
        /// <returns></returns>
        public Result ParseRewriter()
        {
            var result = ParseText(Tree.GetRewriteResult(), Tree.FileName);
            if (result.OK)
                Tree.ResetRewriter();
            return result;
        }

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
