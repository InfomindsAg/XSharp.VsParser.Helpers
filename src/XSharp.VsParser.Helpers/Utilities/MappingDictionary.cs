using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.VsParser.Helpers.Utilities
{
    /// <summary>
    /// Dictionary with a case insensitive stirng as key
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MappingDictionary<T> : Dictionary<string, T>
    {
        /// <summary>
        /// Contructor
        /// </summary>
        public MappingDictionary() : base(StringComparer.OrdinalIgnoreCase)
        { }
    }

    /// <summary>
    /// Dictionary with a case insensitive stirng as key
    /// </summary>
    public class MappingDictionary : Dictionary<string, string>
    {
        /// <summary>
        /// Contructor
        /// </summary>
        public MappingDictionary() : base(StringComparer.OrdinalIgnoreCase)
        { }
    }
}
