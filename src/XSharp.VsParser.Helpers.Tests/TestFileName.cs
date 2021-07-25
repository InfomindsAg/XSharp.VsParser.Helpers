using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.DevTools.XsFormToWinForm.Parser.Tests
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
