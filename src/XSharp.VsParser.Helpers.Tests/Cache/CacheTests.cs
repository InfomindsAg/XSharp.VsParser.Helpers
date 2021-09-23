using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XSharp.VsParser.Helpers.Cache;
using Xunit;

namespace XSharp.Parser.Helpers.Tests.Cache
{
    public class CacheTests
    {
        class DataA
        {
            public int Data { get; set; }
        }

        class DataB
        {
            public List<string> DataList { get; set; }
        }

        [Fact]
        public void CacheTest()
        {
            var fileName = Path.GetTempFileName();
            try
            {
                using var cache = new CacheHelper(fileName);
                {
                    var sourceCodeA = "aaaaaaaaa";

                    cache.TryGetValue("A", sourceCodeA, out DataA dataA).Should().BeFalse();

                    cache.Add("A", sourceCodeA, new DataA { Data = 1 });

                    cache.TryGetValue("A", sourceCodeA, out dataA).Should().BeTrue();
                    dataA.Data.Should().Be(1);

                    cache.TryGetValue("A", sourceCodeA + "X", out dataA).Should().BeFalse();
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        static void CacheTestWithVersion(string baseVersion)
        {
            var fileName = Path.GetTempFileName();
            try
            {
                var sourceCode = "aaaaaaaaa";
                var data = new DataA { Data = 1 };
                using (var cache1 = new CacheHelper(fileName, baseVersion))
                {
                    cache1.Add("A", sourceCode, data);
                }

                using (var cache2 = new CacheHelper(fileName, baseVersion))
                {
                    cache2.TryGetValue("A", sourceCode, out data).Should().BeTrue();
                }

                using (var cache3 = new CacheHelper(fileName, baseVersion + "."))
                {
                    cache3.TryGetValue("A", sourceCode, out data).Should().BeFalse();
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [Fact]
        public void CacheVersionShort()
        {
            CacheTestWithVersion("1");
        }

        [Fact]
        public void CacheVersionLong()
        {
            CacheTestWithVersion("1".PadLeft(10000));
        }

    }
}
