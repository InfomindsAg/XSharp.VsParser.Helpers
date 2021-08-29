using LanguageService.SyntaxTree.Tree;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Linq;

namespace XSharp.VsParser.Helpers.Parser
{
    /// <summary>
    /// Extensions for IParseTree
    /// </summary>
    public static class IParseTreeExtensions
    {
        const string NewList = "$NewLine";

        /// <summary>
        /// Filters an enumeration of IParseTree elements (an Abstract Syntax Tree) based on a context type
        /// </summary>
        /// <typeparam name="T">The context type (ex: MethodContext, Class_Context, ...)</typeparam>
        /// <param name="enumerable">An IEnumerable to filter</param>
        /// <returns>An IEnumerable that contains elements from the input sequence that match the context type</returns>
        public static IEnumerable<T> WhereType<T>(this IEnumerable<IParseTree> enumerable) where T : IParseTree
        {
            foreach (var item in enumerable)
                if (item is T returnItem)
                    yield return returnItem;
        }

        /// <summary>
        /// Filters an enumeration of IParseTree elements (an Abstract Syntax Tree) based on a context type and and a specified condition
        /// </summary>
        /// <typeparam name="T">The context type (ex: MethodContext, Class_Context, ...)</typeparam>
        /// <param name="enumerable">An IEnumerable to filter</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>An IEnumerable that contains elements from the input sequence that match the context type and satisfy the condition</returns>
        public static IEnumerable<T> WhereType<T>(this IEnumerable<IParseTree> enumerable, Func<T, bool> predicate) where T : IParseTree
        {
            var result = enumerable.WhereType<T>();
            if (predicate != null)
                result = result.Where(predicate);
            return result;
        }

        /// <summary>
        /// Filters an enumeration of IParseTree elements (an Abstract Syntax Tree) based on a context type
        /// </summary>
        /// <typeparam name="T">The context type (ex: MethodContext, Class_Context, ...)</typeparam>
        /// <param name="enumerable">An IEnumerable to filter</param>
        /// <returns>An IEnumerable that contains elements and their children from the input sequence that match the context type</returns>
        public static IEnumerable<IParseTree> WhereTypeAndChildren<T>(this IEnumerable<IParseTree> enumerable) where T : IParseTree
        {
            foreach (var item in enumerable.WhereType<T>())
                foreach (var child in item.AsEnumerable())
                    yield return child;
        }

        /// <summary>
        /// Filters an enumeration of IParseTree elements (an Abstract Syntax Tree) based on a context type and and a specified condition
        /// </summary>
        /// <typeparam name="T">The context type (ex: MethodContext, Class_Context, ...)</typeparam>
        /// <param name="enumerable">An IEnumerable to filter</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>An IEnumerable that contains elements and their children from the input sequence that match the context type and satisfy the condition</returns>
        public static IEnumerable<IParseTree> WhereTypeAndChildren<T>(this IEnumerable<IParseTree> enumerable, Func<T, bool> predicate) where T : IParseTree
        {
            foreach (var item in enumerable.WhereType<T>(predicate))
                foreach (var child in item.AsEnumerable())
                    yield return child;
        }

        /// <summary>
        /// Returns the first element of a sequence based on a context type, or null if no element is found.
        /// </summary>
        /// <typeparam name="T">The context type (ex: MethodContext, Class_Context, ...)</typeparam>
        /// <param name="enumerable">An IEnumerable to return the first element of</param>
        /// <returns>The first element from the input sequence that match the context type or null  if no element is found</returns>
        public static T FirstOrDefaultType<T>(this IEnumerable<IParseTree> enumerable) where T : IParseTree
            => enumerable.WhereType<T>().FirstOrDefault();

        /// <summary>
        /// Returns the first parent element element of an element based on a context type, or null if no element is found.
        /// </summary>
        /// <typeparam name="T">The context type (ex: MethodContext, Class_Context, ...)</typeparam>
        /// <param name="element">The start element</param>
        /// <returns>The first parent element of the element, that match the context type or null if no element is found</returns>
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

        /// <summary>
        /// Returns an IEnumerable of the element and all it's children
        /// </summary>
        /// <param name="element">The start element</param>
        /// <returns>Returns an IEnumerable of the element and all it's children</returns>
        public static IEnumerable<IParseTree> AsEnumerable(this IParseTree element)
            => new ParseTreeEnumerable(element);

        /// <summary>
        /// Returns an element relative positioned of the current element
        /// </summary>
        /// <param name="element">The start element</param>
        /// <param name="relativePosition">The relative position of the requested element</param>
        /// <returns>The relative positioned element or null</returns>
        public static IParseTree RelativePositionedChildInParentOrDefault(this IParseTree element, int relativePosition)
        {
            var list = element.Parent.AsEnumerable().ToList();
            return list.ElementAtOrDefault(list.IndexOf(element) + relativePosition);
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
