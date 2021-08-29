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
    }
}
