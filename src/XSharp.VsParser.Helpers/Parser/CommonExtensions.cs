using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.VsParser.Helpers.Parser
{
    /// <summary>
    /// Common Extensions
    /// </summary>
    public static class CommonExtensions
    {
        /// <summary>
        /// Checks, if thow string values are equal when the casing is ignored
        /// </summary>
        /// <param name="value1">First string</param>
        /// <param name="value2">Second string</param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string value1, string value2)
            => value1?.Equals(value2, StringComparison.OrdinalIgnoreCase) == true;
    }
}
