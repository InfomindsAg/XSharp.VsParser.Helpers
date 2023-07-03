using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XSharp.VsParser.Helpers.Extensions
{
    /// <summary>
    /// Extensions for FileInfoEnumerable classes
    /// </summary>
    public static class FileInfoEnumerableExtensions
    {
        /// <summary>
        /// Returns the fargest files first
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static IEnumerable<FileInfo> OrderByLargestFiles(this IEnumerable<FileInfo> files)
            => files.OrderByDescending(q => q.Length);
    }
}
