using System;
using System.Collections.Generic;
using System.Text;
using XSharp.VsParser.Helpers.Values;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser
{
    public static partial class ToValuesExtensions
    {

        public static ClassContextValues ToValues(this Class_Context context) => ClassContextValues.Build(context);

        public static MethodContextValues ToValues(this MethodContext context) => MethodContextValues.Build(context);
        

        // Todo: Add Conversions for Proc/Func, Properties, Access/Assign, ...


    }


}
