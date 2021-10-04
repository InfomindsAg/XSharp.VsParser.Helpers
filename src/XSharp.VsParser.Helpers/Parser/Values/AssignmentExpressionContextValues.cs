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
        /// The AccessMember for the left side of the assignment
        /// </summary>
        public AccessMemberContextValues AssignToAccessMember { get; internal set; }

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
                AssignToAccessMember = AccessMemberContextValues.Build(accessMember),
                ValueExpression = context.Right?.GetText()
            };
        }
    }
}
