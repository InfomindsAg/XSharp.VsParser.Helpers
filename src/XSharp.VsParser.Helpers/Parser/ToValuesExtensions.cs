using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XSharp.VsParser.Helpers.Values;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser
{
    public static partial class ToValuesExtensions
    {

        public static ClassContextValues ToValues(this Class_Context context) => ClassContextValues.Build(context);
        public static IEnumerable<ClassContextValues> ToValues(this IEnumerable<Class_Context> enumerable) => enumerable.Select(ClassContextValues.Build);

        public static MethodContextValues ToValues(this MethodContext context) => MethodContextValues.Build(context);
        public static IEnumerable<MethodContextValues> ToValues(this IEnumerable<MethodContext> enumerable) => enumerable.Select(MethodContextValues.Build);

        public static SuperExpressionContextValues ToValues(this SuperExpressionContext context) => SuperExpressionContextValues.Build(context);
        public static IEnumerable<SuperExpressionContextValues> ToValues(this IEnumerable<SuperExpressionContext> enumerable) => enumerable.Select(SuperExpressionContextValues.Build);



        // Todo: Add Conversions for Proc/Func, Properties, Access/Assign, ...


    }


}
