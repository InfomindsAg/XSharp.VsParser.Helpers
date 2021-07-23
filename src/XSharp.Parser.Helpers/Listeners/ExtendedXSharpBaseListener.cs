using LanguageService.CodeAnalysis.XSharp.SyntaxParser;
using LanguageService.SyntaxTree;
using LanguageService.SyntaxTree.Misc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.Parser.Helpers.Listeners
{
    public class ExtendedXSharpBaseListener : XSharpBaseListener
    {

        protected ParserContext Current;

        public override void EnterSource([NotNull] SourceContext context)
        {
            Current.Clear();

            base.EnterSource(context);
        }

        #region Overriden Listener Methods

        public override void EnterClass_([NotNull] Class_Context context)
        {
            base.EnterClass_(context);

            Current.ClassName = context.identifier()?.GetText();
            Current.InheritsClassName = context.BaseType?.GetText();
        }

        public override void EnterMethod([NotNull] MethodContext context)
        {
            base.EnterMethod(context);

            Current.MethodName = context.signature().identifier()?.GetText();
        }

        #endregion

    }
}
