using LanguageService.CodeAnalysis.XSharp;
using LanguageService.CodeAnalysis.XSharp.SyntaxParser;
using LanguageService.SyntaxTree;
using LanguageService.SyntaxTree.Tree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Linq;

namespace XSharp.VsParser.Helpers.Parser
{
    public class ParserHelper
    {
        readonly GenericErrorListener _ErrorListener = new();
        readonly XSharpParseOptions _XSharpOptions;
        private BufferedTokenStream _XSharpTokenStream;

        public AbstractSyntaxTree Tree { get; private set; }

        [Obsolete("Replaced by Tree Property")]
        public AbstractSyntaxTree SourceTree => Tree;

        public List<IToken> Comments => _XSharpTokenStream?.GetTokens().Where(q => XSharpLexer.IsComment(q.Type)).ToList();

        internal ParserHelper(XSharpParseOptions xsharpOptions)
        {
            XSharpSpecificCompilationOptions.SetDefaultIncludeDir(@"c:\Program Files(x86)\XSharp\Include\");
            XSharpSpecificCompilationOptions.SetWinDir(Environment.GetFolderPath(Environment.SpecialFolder.Windows));
            XSharpSpecificCompilationOptions.SetSysDir(Environment.GetFolderPath(Environment.SpecialFolder.System));

            _XSharpOptions = xsharpOptions;

            Tree = null;
        }

        public void Clear()
        {
            _ErrorListener.Clear();
            Tree = null;
            _XSharpTokenStream = null;
        }

        public Result ParseFile(string fileName)
            => ParseText(File.ReadAllText(fileName), fileName);

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
                }
            }
            catch (Exception ex)
            {
                _ErrorListener.Result.Errors.Add(new Result.Item { Message = "Exception: " + ex.Message, Line = 0 });
            }

            return _ErrorListener.Result;
        }

        public Result ParseRewriter()
            => ParseText(Tree.Rewriter.GetText(), Tree.FileName);

        #region Builders

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

        public static ParserHelper BuildWithOptionsList(List<string> options)
            => new(XSharpParseOptions.FromVsValues(options));

        #endregion
    }
}
