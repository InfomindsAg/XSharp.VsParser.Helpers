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
using XSharp.VsParser.Helpers.Rewriters;

namespace XSharp.VsParser.Helpers.Parser
{
    public class ParserHelper
    {
        const string NewList = "$NewLine";
        GenericErrorListener _ErrorListener = new();
        string _SourceCode;
        XSharpParseOptions _XSharpOptions;

        protected internal ITokenStream _Tokens;
        protected internal XSharpParserRuleContext _StartRule;
        protected string _FileName;

        public AbstractSyntaxTree SourceTree { get; }

        void CheckParseSuccessful()
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

        public (bool changed, string newSourceCode) ExecuteRewriters(List<XSharpBaseRewriter> rewriters)
        {
            Debug.Assert((rewriters?.Count ?? 0) > 0, "List of rewriters can not be empty");
            CheckParseSuccessful();

            var tokenRewriter = new TokenStreamRewriter(_Tokens);

            foreach (var rewriter in rewriters)
            {
                rewriter.Initialize(tokenRewriter);
                ParseTreeWalker.Default.Walk(rewriter, _StartRule);
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


        /// <summary>
        /// Dumps the AST created by parsing as XDocument
        /// </summary>
        public XDocument DumpXml()
        {
            CheckParseSuccessful();

            static void DumpTerminalValue(TerminalNodeImpl terminalNodeImpl, XElement element)
            {
                var text = terminalNodeImpl.Payload.Text;
                if (text == Environment.NewLine)
                    element.Add(NewList);
                else if (text.IndexOfAny(new char[] { '\n', '<', '>', '&' }) == -1)
                    element.Add(text);
                else
                    element.Add(new XCData(text));
            }

            static void DumpElement(IParseTree rule, XContainer container)
            {
                var node = new XElement(rule.GetType().Name);
                container.Add(node);

                if (rule is TerminalNodeImpl terminalNodeImpl && rule.ChildCount == 0)
                    DumpTerminalValue(terminalNodeImpl, node);

                for (int i = 0; i < rule.ChildCount; i++)
                    DumpElement(rule.GetChild(i), node);
            }

            var doc = new XDocument();
            DumpElement(_StartRule, doc);
            return doc;
        }


        /// <summary>
        /// Dumps the AST created by parsing as Yaml
        /// </summary>
        public string DumpYaml()
        {
            CheckParseSuccessful();
            var sb = new StringBuilder();

            void DumpTerminalValue(string text, int indent)
            {
                if (text == Environment.NewLine)
                {
                    sb.AppendLine(" " + NewList);
                    return;
                }

                text = text.Replace("\\", "\\\\").Replace("\"", "\\\"");
                if (text.Contains("\n"))
                {
                    sb.AppendLine(" |-");
                    foreach (var line in text.Replace("\r", "").Split('\n'))
                        sb.AppendLine(new string(' ', indent * 2) + line);
                }
                else
                    sb.Append(" \"").Append(text).AppendLine("\"");
            }

            void DumpElement(IParseTree rule, int indent)
            {
                var indentString = new string(' ', indent * 2);
                indent++;

                sb.Append(indentString).Append("- ").Append(rule.GetType().Name).Append(":");

                if (rule is TerminalNodeImpl terminalNodeImpl && rule.ChildCount == 0)
                    DumpTerminalValue(terminalNodeImpl.Payload.Text, indent + 1);
                else
                {
                    sb.AppendLine();
                    for (int i = 0; i < rule.ChildCount; i++)
                        DumpElement(rule.GetChild(i), indent);
                }
            }

            sb.Append(_StartRule.GetType().Name).AppendLine(":");
            for (int i = 0; i < _StartRule.ChildCount; i++)
                DumpElement(_StartRule.GetChild(i), 0);
            return sb.ToString();
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
