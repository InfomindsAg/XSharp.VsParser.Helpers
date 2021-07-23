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

        protected ParserContext Current = new();

        public override void EnterSource([NotNull] SourceContext context)
        {
            Current.Clear();

            base.EnterSource(context);
        }

        #region Overriden Listener Methods

        public override void EnterClass_([NotNull] Class_Context context)
        {
            base.EnterClass_(context);

            Current.Class = new ParserContext.ClassContext { Name = context.identifier()?.GetText(), Inherits = context.BaseType?.GetText() };
        }

        public override void EnterMethod([NotNull] MethodContext context)
        {
            base.EnterMethod(context);

            Current.Method = new ParserContext.MethodContext { Name = context.signature().identifier()?.GetText() };
        }

        public override void ExitMethod([NotNull] MethodContext context)
        {
            base.ExitMethod(context);

            Current.Method = null;
        }

        public override void ExitClass_([NotNull] Class_Context context)
        {
            base.ExitClass_(context);

            Current.Class = null;
        }

        #endregion

    }
}
