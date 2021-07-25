using LanguageService.CodeAnalysis.XSharp;
using LanguageService.CodeAnalysis.XSharp.SyntaxParser;
using LanguageService.SyntaxTree;
using LanguageService.SyntaxTree.Tree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace XSharp.VsParser.Helpers.Parser
{
    public class ParserHelper
    {
        GenericErrorListener _ErrorListener = new();
        string _SourceCode;
        XSharpParseOptions _XSharpOptions;

        protected internal ITokenStream _Tokens;
        protected internal XSharpParserRuleContext _StartRule;
        protected string _FileName;

        public AbstractSyntaxTree SourceTree { get; }

        internal void CheckParseSuccessful()
        {
            if (_StartRule == null)
                throw new ArgumentException("Parsing was not successful");
        }

        internal ParserHelper(XSharpParseOptions xsharpOptions)
        {
            XSharpSpecificCompilationOptions.SetDefaultIncludeDir(@"c:\Program Files(x86)\XSharp\Include\");
            XSharpSpecificCompilationOptions.SetWinDir(Environment.GetFolderPath(Environment.SpecialFolder.Windows));
            XSharpSpecificCompilationOptions.SetSysDir(Environment.GetFolderPath(Environment.SpecialFolder.System));

            _XSharpOptions = xsharpOptions;

            SourceTree = new AbstractSyntaxTree(this);
        }

        public void Clear()
        {
            _SourceCode = null;
            _Tokens = null;
            _StartRule = null;
            _FileName = null;
            _ErrorListener.Clear();
            SourceTree.Clear();
        }

        public Result ParseFile(string fileName)
            => ParseText(File.ReadAllText(fileName), fileName);

        public Result ParseText(string sourceCode, string fileName)
        {
            Clear();

            _SourceCode = sourceCode;

            var ok = XSharp.Parser.VsParser.Parse(_SourceCode, fileName, _XSharpOptions, _ErrorListener, out _Tokens, out _StartRule);
            if (!ok && _ErrorListener.Result.OK)
                _ErrorListener.Result.Errors.Add(new Result.Item { Message = "Generic Parse Error", Line = 0 });

            if (!_ErrorListener.Result.OK)
            {
                _Tokens = null;
                _StartRule = null;
            }
            else
                _FileName = fileName;

            return _ErrorListener.Result;
        }

        public void ExecuteListeners(List<XSharpBaseListener> listeners)
        {
            Debug.Assert((listeners?.Count ?? 0) > 0, "List of listeners can not be empty");
            CheckParseSuccessful();

            foreach (var listener in listeners)
                ParseTreeWalker.Default.Walk(listener, _StartRule);
        }

        #region Builders

        public static ParserHelper BuildWithVoDefaultOptions()
                 => new ParserHelper(XSharpParseOptions.FromVsValues(new List<string>
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
            => new ParserHelper(XSharpParseOptions.FromVsValues(options));

        #endregion
    }
}
