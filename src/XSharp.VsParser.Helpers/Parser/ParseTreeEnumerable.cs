using LanguageService.SyntaxTree.Tree;
using System;
using System.Collections;
using System.Collections.Generic;

namespace XSharp.VsParser.Helpers.Parser
{
    internal class ParseTreeEnumerable : IEnumerable<IParseTree>
    {
        readonly IParseTree _Start;
        internal ParseTreeEnumerable(IParseTree start)
            => _Start = start;

        IEnumerator<IParseTree> GetEnumerator(IParseTree start)
        {
            yield return start;
            for (int i = 0; i < start.ChildCount; i++)
            {
                var childEnumerator = GetEnumerator(start.GetChild(i));
                while (childEnumerator.MoveNext())
                    yield return childEnumerator.Current;
            }
        }

        public IEnumerator<IParseTree> GetEnumerator()
            => GetEnumerator(_Start);

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
