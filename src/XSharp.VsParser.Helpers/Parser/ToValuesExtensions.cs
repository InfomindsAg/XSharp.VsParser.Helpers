using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XSharp.VsParser.Helpers.Parser.Values;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser
{
    public static partial class ToValuesExtensions
    {

        public static ClassContextValues ToValues(this Class_Context context) => ClassContextValues.Build(context);
        public static IEnumerable<ClassContextValues> ToValues(this IEnumerable<Class_Context> enumerable) => enumerable.Select(ClassContextValues.Build);

        public static MethodContextValues ToValues(this MethodContext context) => MethodContextValues.Build(context);
        public static IEnumerable<MethodContextValues> ToValues(this IEnumerable<MethodContext> enumerable) => enumerable.Select(MethodContextValues.Build);

        public static ParameterContextValues ToValues(this ParameterContext context) => ParameterContextValues.Build(context);
        public static IEnumerable<ParameterContextValues> ToValues(this IEnumerable<ParameterContext> enumerable) => enumerable.Select(ParameterContextValues.Build);

        public static SuperExpressionContextValues ToValues(this SuperExpressionContext context) => SuperExpressionContextValues.Build(context);
        public static IEnumerable<SuperExpressionContextValues> ToValues(this IEnumerable<SuperExpressionContext> enumerable) => enumerable.Select(SuperExpressionContextValues.Build);

        public static ReturnStmtContextValues ToValues(this ReturnStmtContext context) => ReturnStmtContextValues.Build(context);
        public static IEnumerable<ReturnStmtContextValues> ToValues(this IEnumerable<ReturnStmtContext> enumerable) => enumerable.Select(ReturnStmtContextValues.Build);

        // Todo: Add Conversions for Proc/Func, Properties, Access/Assign, ...


    }


}
