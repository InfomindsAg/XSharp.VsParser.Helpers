using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using UtfUnknown;

namespace XSharp.VsParser.Helpers.FileEncoding
{
    /// <summary>
    /// Helper Class for file encoding detection
    /// </summary>
    public class FileEncodingHelper
    {
        readonly HashSet<Encoding> _LikelyEncodings;

        /// <summary>
        /// Creates a new FileEncodingHelper
        /// </summary>
        /// <param name="likelyEncodings">A list of the most likely encodings.</param>
        public FileEncodingHelper(List<Encoding> likelyEncodings = null)
            => _LikelyEncodings = new HashSet<Encoding>(likelyEncodings ?? Enumerable.Empty<Encoding>());

        /// <summary>
        /// Detects the encoding of a file
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <returns>The detected encoding's</returns>
        public FileEncoding DetectFileEncoding(string fileName)
        {
            var result = new FileEncoding();

            if (!File.Exists(fileName) || new FileInfo(fileName).Length == 0)
                return result;

            var detectedEncoding = CharsetDetector.DetectFromFile(fileName);
            if (detectedEncoding == null)
                return result;

            if (detectedEncoding.Details?.Count > 0)
            {
                result.DetectedEncodings = detectedEncoding.Details.Select(q => new FileEncoding.DetectionResult { Encoding = q.Encoding, Confidence = q.Confidence }).ToArray();
                result.Encoding = null;

                if (detectedEncoding.Details.Count > 1 && _LikelyEncodings.Count > 0)
                {
                    var likelyEncoding = result.DetectedEncodings
                                            .Where(q => _LikelyEncodings.Contains(q.Encoding) && q.Confidence > 0.3)
                                            .OrderByDescending(q => q.Confidence)
                                            .FirstOrDefault();

                    if (likelyEncoding != null)
                        result.Encoding = likelyEncoding.Encoding;
                }

                result.Encoding ??= detectedEncoding.Detected?.Encoding ?? Encoding.UTF8;
            }

            if (result.Encoding == Encoding.UTF8)
            {
                using var fileStream = new FileStream(fileName, FileMode.Open);
                if (fileStream.Length > 3)
                {
                    var bits = new byte[3];
                    fileStream.Read(bits, 0, 3);

                    var utf8Bom = (bits[0] == 0xEF && bits[1] == 0xBB && bits[2] == 0xBF);
                    if (!utf8Bom)
                        result.Encoding = new UTF8Encoding(false);
                }
            }

            return result;
        }
    }

    /// <summary>
    /// FileEncoding Result
    /// </summary>
    public class FileEncoding
    {
        /// <summary>
        /// Detected Result
        /// </summary>
        public class DetectionResult
        {
            /// <summary>
            /// The detected Encoding
            /// </summary>
            public Encoding Encoding { get; set; }

            /// <summary>
            /// The confidence of the detection
            /// </summary>
            public float Confidence { get; set; }
        }

        /// <summary>
        /// The most likely file encoding
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// The list of all the detected encodings
        /// </summary>
        public DetectionResult[] DetectedEncodings { get; set; } = new DetectionResult[0];
    }
}
