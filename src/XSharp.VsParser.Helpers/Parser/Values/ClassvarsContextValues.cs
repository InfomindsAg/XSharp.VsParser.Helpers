using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using XSharp.VsParser.Helpers.Parser.Values.Interfaces;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser.Values
{

    /// <summary>
    /// Values class for a Classvars instance
    /// </summary>
    public class ClassvarsContextValues : ContextValues<ClassvarsContext>
    {
        /// <summary>
        /// The modifiers
        /// </summary>
        public string[] Modifiers { get; private set; }

        /// <summary>
        /// The array of variables
        /// </summary>
        public ClassvarContextValues[] Vars { get; private set; }

        static internal ClassvarsContextValues Build(ClassvarsContext context)
        {
            if (context == null)
                return null;
            var variables = context._Vars;
            var varsNoDatatype = new List<ClassvarContext>();

            foreach (var variable in variables)
            {
                if (variable.DataType == null)
                {
                    varsNoDatatype.Add(variable);
                    continue;
                }

                varsNoDatatype.ForEach(v => v.DataType = variable.DataType);
                varsNoDatatype.Clear();
            }

            return new ClassvarsContextValues
            {
                Context = context,
                Modifiers = (context.Modifiers?._Tokens?.Select(q => q.Text) ?? Enumerable.Empty<string>()).ToArray(),
                Vars = variables.Select(x => ClassvarContextValues.Build(x)).ToArray()
            };
        }
    }
}
