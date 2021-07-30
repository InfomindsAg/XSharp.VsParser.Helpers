using XSharp.VsParser.Helpers.Parser;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Values
{
    public class SuperExpressionContextValues : ContextValues<SuperExpressionContext>
    {
        public string MethodName { get; internal set; }

        static internal SuperExpressionContextValues Build(SuperExpressionContext context)
        {
            if (context == null)
                return null;

            var accessMember = context.FirstParentOrDefault<AccessMemberContext>();
            // var signature = context.;
            return new SuperExpressionContextValues
            {
                Context = context,
                MethodName = accessMember.simpleName().GetText(),
            };
        }
    }
}
