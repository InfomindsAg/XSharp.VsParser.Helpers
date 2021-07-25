using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace XSharp.Parser.Helpers.Tests
{
    static class TestFileName
    {
        public static string UnitTestData(string fileName)
            => Path.Combine("UnitTestData", fileName);

        public static string CodeFile(string fileName)
            => UnitTestData(Path.Combine("CodeFiles", fileName));

        public static string ProjectFile(string fileName)
            => UnitTestData(Path.Combine("ProjectFiles", fileName));
    }
}
