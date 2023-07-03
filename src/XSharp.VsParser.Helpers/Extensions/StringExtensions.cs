using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSharp.VsParser.Helpers.Extensions
{
    /// <summary>
    /// Common Extensions
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Checks, if thow string values are equal when the casing is ignored
        /// </summary>
        /// <param name="value1">First string</param>
        /// <param name="value2">Second string</param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string value1, string value2)
            => value1?.Equals(value2, StringComparison.OrdinalIgnoreCase) == true;

        /// <summary>
        /// Determines whether the end of this string instance matches a specified string (CaseInsensitive).
        /// </summary>
        /// <param name="value1">The string</param>
        /// <param name="value2">The string to compare to the substring at the end of this instance.</param>
        /// <returns></returns>
        public static bool EndsWithIgnoreCase(this string value1, string value2)
            => value1?.EndsWith(value2, StringComparison.OrdinalIgnoreCase) == true;

        /// <summary>
        /// Returns a value indicating whether a specified substring occurs within this string (CaseInsensitive).
        /// </summary>
        /// <param name="value1">The string</param>
        /// <param name="value2">The string to compare to the substring at the end of this instance.</param>
        /// <returns></returns>
        public static bool ContainsIgnoreCase(this string value1, string value2)
            => value1?.IndexOf(value2, StringComparison.OrdinalIgnoreCase) >= 0;

        /// <summary>
        /// Returns a value indicating whether a specified string occurs within string array (CaseInsensitive).
        /// </summary>
        /// <param name="valueArray">The string</param>
        /// <param name="value">The string to compare to the substring at the end of this instance.</param>
        /// <returns></returns>
        public static bool ContainsIgnoreCase(this string[] valueArray, string value)
            => valueArray?.Contains(value, StringComparer.OrdinalIgnoreCase) == true;
    }
}
