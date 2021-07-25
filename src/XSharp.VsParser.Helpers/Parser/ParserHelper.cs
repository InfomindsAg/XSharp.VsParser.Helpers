using IM.DevTools.XsFormToWinForm.Parser.Rewriters;
using LanguageService.CodeAnalysis.XSharp;
using LanguageService.CodeAnalysis.XSharp.SyntaxParser;
using LanguageService.SyntaxTree;
using LanguageService.SyntaxTree.Tree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using XSharp.Parser.Helpers.Parser;

namespace XSharp.Parser.Helpers
{
    public class ParserHelper
    {
        GenericErrorListener _ErrorListener = new();
        string _SourceCode;
        XSharpParseOptions _XSharpOptions;

        protected ITokenStream _Tokens;
        protected XSharpParserRuleContext _StartRule;
        protected bool _ParseSuccessful;
        protected string _FileName;

        internal ParserHelper(XSharpParseOptions xsharpOptions)
        {
            XSharpSpecificCompilationOptions.SetDefaultIncludeDir(@"c:\Program Files(x86)\XSharp\Include\");
            XSharpSpecificCompilationOptions.SetWinDir(Environment.GetFolderPath(Environment.SpecialFolder.Windows));
            XSharpSpecificCompilationOptions.SetSysDir(Environment.GetFolderPath(Environment.SpecialFolder.System));

            _XSharpOptions = xsharpOptions;
        }

        public void Clear()
        {
            _SourceCode = null;
            _Tokens = null;
            _StartRule = null;
            _ParseSuccessful = false;
            _FileName = null;
            _ErrorListener.Clear();
        }

        public Result ParseFile(string fileName)
        {
            _ErrorListener.Clear();
            _SourceCode = File.ReadAllText(fileName);

            var ok = VsParser.Parse(_SourceCode, fileName, _XSharpOptions, _ErrorListener, out _Tokens, out _StartRule);
            if (!ok && _ErrorListener.Result.OK)
                _ErrorListener.Result.Errors.Add(new Result.Item { Message = "Generic Parse Error", Line = 0 });

            if (!_ErrorListener.Result.OK)
            {
                _Tokens = null;
                _StartRule = null;
            }
            else
                _FileName = fileName;

            _ParseSuccessful = _ErrorListener.Result.OK;
            return _ErrorListener.Result;
        }

        public void ExecuteListeners(List<XSharpBaseListener> listeners)
        {
            Debug.Assert((listeners?.Count ?? 0) > 0, "List of listeners can not be empty");
            if (!_ParseSuccessful)
                throw new ArgumentException("Parsing was not successful");

            var walker = new ParseTreeWalker();
            foreach (var listener in listeners)
                walker.Walk(listener, _StartRule);
        }

        public (bool changed, string newSourceCode) ExecuteRewriters(List<XSharpBaseRewriter> rewriters)
        {
            Debug.Assert((rewriters?.Count ?? 0) > 0, "List of rewriters can not be empty");
            if (!_ParseSuccessful)
                throw new ArgumentException("Parsing was not successful");

            var tokenRewriter = new TokenStreamRewriter(_Tokens);
            var walker = new ParseTreeWalker();

            foreach (var rewriter in rewriters)
            {
                rewriter.Initialize(tokenRewriter);
                walker.Walk(rewriter, _StartRule);
            }

            var newSourceCode = tokenRewriter.GetText();
            return (_SourceCode != newSourceCode, newSourceCode);
        }

        public bool ExecuteRewriters(List<XSharpBaseRewriter> rewriters, string newFilename = null)
        {
            var (changed, newSourceCode) = ExecuteRewriters(rewriters);
            if (changed)
            {
                newFilename ??= _FileName;
                File.WriteAllText(newFilename, newSourceCode);
            }

            return changed;
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
