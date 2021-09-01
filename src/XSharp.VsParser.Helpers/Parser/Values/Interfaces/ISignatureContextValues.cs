using System.Linq;

namespace XSharp.VsParser.Helpers.Parser.Values.Interfaces
{
    /// <summary>
    /// Interface for SignatureContextValues
    /// </summary>
    public interface ISignatureContextValues
    {
        /// <summary>
        /// The CallingConvention
        /// </summary>
        string CallingConvention { get; }
        /// <summary>
        /// The method name
        /// </summary>
        string Name { get; }
        /// <summary>
        /// An array with the parameter values
        /// </summary>
        ParameterContextValues[] Parameters { get; }
        /// <summary>
        /// The return tyoe
        /// </summary>
        string ReturnType { get; }
    }
}
