using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;
using System.Linq;

namespace XSharp.VsParser.Helpers.Parser.Values
{
    /// <summary>
    /// Values class for a AssignmentExpressionContext instance
    /// </summary>
    public class AssignmentExpressionContextValues : ContextValues<AssignmentExpressionContext>
    {
        /// <summary>
        /// The expression to access the property
        /// </summary>
        public string PropertyAccessExpression { get; internal set; }

        /// <summary>
        /// The property name
        /// </summary>
        public string PropertyName { get; internal set; }

        /// <summary>
        /// The expression assigned to the propety
        /// </summary>
        public string ValueExpression { get; internal set; }

        static internal AssignmentExpressionContextValues Build(AssignmentExpressionContext context)
        {
            if (context == null)
                return null;

            var accessMember = context.Left.AsEnumerable().FirstOrDefaultType<AccessMemberContext>();
            return new AssignmentExpressionContextValues
            {
                Context = context,
                PropertyAccessExpression = accessMember?.Expr?.GetText(),
                PropertyName = accessMember?.Name?.GetText(),
                ValueExpression = context.Right?.GetText()
            };
        }
    }
}
