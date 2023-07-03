using System;
using System.Collections.Generic;
using System.Linq;

namespace XSharp.VsParser.Helpers.Utilities
{
    /// <summary>
    /// A Hashset Class for case-insensitive names
    /// </summary>
    public class NameHashset : HashSet<string>
    {
        /// <summary>
        /// Constructur
        /// </summary>
        public NameHashset() : base(StringComparer.OrdinalIgnoreCase)
        {

        }

        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="items"></param>
        public NameHashset(IEnumerable<string> items) : this()
        {
            foreach (string item in items ?? Enumerable.Empty<string>())
                Add(item);
        }
    }
}
