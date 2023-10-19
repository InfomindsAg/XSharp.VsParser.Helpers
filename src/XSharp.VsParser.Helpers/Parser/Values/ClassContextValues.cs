using System.Linq;
using XSharp.VsParser.Helpers.Extensions;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser.Values
{
    /// <summary>
    /// Values class for a Class_Context instance
    /// </summary>
    public class ClassContextValues : ContextValues<Class_Context>
    {
        // TODO: Extend with Implents, Modifiers, Attributes

        /// <summary>
        /// The class name
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// The base class for the class
        /// </summary>
        public string Inherits { get; internal set; }

        /// <summary>
        /// The interfaces, that the class implements
        /// </summary>
        public string[] Implements { get; internal set; }

        /// <summary>
        /// The modifiers
        /// </summary>
        public string[] Modifiers { get; private set; }

        /// <summary>
        /// True, if the class is marked with the public modifier
        /// </summary>
        public bool IsPublic => Modifiers.Any(q => q.EqualsIgnoreCase("public"));
        /// <summary>
        /// True, if the class is marked with the protected modifier
        /// </summary>
        public bool IsProtected => Modifiers.Any(q => q.EqualsIgnoreCase("protected"));
        /// <summary>
        /// True, if the class is marked with the private modifier
        /// </summary>
        public bool IsPrivate => Modifiers.Any(q => q.EqualsIgnoreCase("private"));
        /// <summary>
        /// True, if the class is marked with the abstract modifier
        /// </summary>
        public bool IsAbstract => Modifiers.Any(q => q.EqualsIgnoreCase("abstract"));
        /// <summary>
        /// True, if the class is marked with the partial modifier
        /// </summary>
        public bool IsPartial => Modifiers.Any(q => q.EqualsIgnoreCase("partial"));
        /// <summary>
        /// True, if the class is marked with the export modifier
        /// </summary>
        public bool IsExport => Modifiers.Any(q => q.EqualsIgnoreCase("export"));
        /// <summary>
        /// True, if the class is marked with the hidden modifier
        /// </summary>
        public bool IsHidden => Modifiers.Any(q => q.EqualsIgnoreCase("hidden"));
        /// <summary>
        /// True, if the class is marked with the internal modifier
        /// </summary>
        public bool IsInternal => Modifiers.Any(q => q.EqualsIgnoreCase("internal"));
        /// <summary>
        /// True, if the class is marked with the sealed modifier
        /// </summary>
        public bool IsSealed => Modifiers.Any(q => q.EqualsIgnoreCase("sealed"));
        /// <summary>
        /// True, if the class is marked with the static modifier
        /// </summary>
        public bool IsStatic => Modifiers.Any(q => q.EqualsIgnoreCase("static"));
        /// <summary>
        /// True, if the class is marked with the unsafe modifier
        /// </summary>
        public bool IsUnsafe => Modifiers.Any(q => q.EqualsIgnoreCase("unsafe"));


        static internal ClassContextValues Build(Class_Context context)
        {
            if (context == null)
                return null;

            return new ClassContextValues
            {
                Context = context,
                Name = context.identifier()?.GetText(),
                Inherits = context.BaseType?.GetText(),
                Implements = (context._Implements?.Select(q => q.GetText()) ?? Enumerable.Empty<string>()).ToArray(),
                Modifiers = (context.Modifiers?._Tokens?.Select(q => q.Text) ?? Enumerable.Empty<string>()).ToArray(),
            };
        }
    }
}
