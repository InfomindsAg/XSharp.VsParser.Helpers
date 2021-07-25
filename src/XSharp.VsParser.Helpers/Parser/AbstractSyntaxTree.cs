using LanguageService.CodeAnalysis.XSharp.SyntaxParser;
using LanguageService.SyntaxTree.Tree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace XSharp.VsParser.Helpers.Parser
{
    public class AbstractSyntaxTree : IEnumerable<IParseTree>
    {
        readonly XSharpParserRuleContext _Start;

        internal AbstractSyntaxTree(XSharpParserRuleContext start)
        {
            _Start = start;
        }

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
        {
            return GetEnumerator(_Start);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator(_Start);
        }

    }
}
