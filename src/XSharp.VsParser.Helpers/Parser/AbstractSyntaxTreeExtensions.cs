using LanguageService.SyntaxTree.Tree;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Linq;
using System.Linq.Expressions;

namespace XSharp.VsParser.Helpers.Parser
{
    public static class AbstractSyntaxTreeExtensions
    {
        const string NewList = "$NewLine";

        public static IEnumerable<T> WhereType<T>(this IEnumerable<IParseTree> enumerable) where T : IParseTree
        {
            foreach (var item in enumerable)
                if (item is T returnItem)
                    yield return returnItem;
        }

        public static IEnumerable<T> WhereType<T>(this IEnumerable<IParseTree> enumerable, Func<T, bool> predicate) where T : IParseTree
        {
            var result = enumerable.WhereType<T>();
            if (predicate == null)
                result = result.Where(predicate);
            return result;
        }
        public static T FirstOrDefaultType<T>(this IEnumerable<IParseTree> enumerable) where T : IParseTree
            => enumerable.WhereType<T>().FirstOrDefault();

        public static T FirstParentOrDefault<T>(this IParseTree element) where T : IParseTree
        {
            element = element?.Parent;
            while (element != null)
            {
                if (element is T returnElement)
                    return returnElement;
                element = element?.Parent;
            }

            return default;
        }

        public static IEnumerable<IParseTree> AsEnumerable(this IParseTree source)
            => new ParseTreeEnumerable(source);

        public static IParseTree RelativePositionedChildInParentOrDefault(this IParseTree source, int relativePosition)
        {
            var list = source.Parent.AsEnumerable().ToList();
            return list.ElementAtOrDefault(list.IndexOf(source) + relativePosition);
        }

        /// <summary>
        /// Dumps the AST created by parsing as XDocument
        /// </summary>
        public static XDocument DumpXml(this IParseTree startRule)
        {
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
            DumpElement(startRule, doc);
            return doc;
        }


        /// <summary>
        /// Dumps the AST created by parsing as Yaml
        /// </summary>
        public static string DumpYaml(this IParseTree startRule)
        {
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

            sb.Append(startRule.GetType().Name).AppendLine(":");
            for (int i = 0; i < startRule.ChildCount; i++)
                DumpElement(startRule.GetChild(i), 0);
            return sb.ToString();
        }
    }
}
