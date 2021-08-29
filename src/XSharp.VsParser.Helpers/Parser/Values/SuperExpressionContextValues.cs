using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser.Values
{
    /// <summary>
    /// Values class for a SuperExpressionContext instance
    /// </summary>
    public class SuperExpressionContextValues : ContextValues<SuperExpressionContext>
    {
        /// <summary>
        /// The method name for the super call
        /// </summary>
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
