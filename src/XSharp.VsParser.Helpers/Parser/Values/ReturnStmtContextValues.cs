using XSharp.VsParser.Helpers.Parser;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser.Values
{
    public class ReturnStmtContextValues : ContextValues<ReturnStmtContext>
    {
        public string ExpressionText { get; internal set; }

        static internal ReturnStmtContextValues Build(ReturnStmtContext context)
        {
            if (context == null)
                return null;

            //var accessMember = context.FirstParentOrDefault<AccessMemberContext>();
            // var signature = context.;
            return new ReturnStmtContextValues
            {
                Context = context,
                ExpressionText = context.Expr?.GetText(),
            };
        }
    }
}
