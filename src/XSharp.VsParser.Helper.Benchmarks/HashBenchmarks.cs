using BenchmarkDotNet.Attributes;
using K4os.Hash.xxHash;
using System.Text;

public class HashBenchmarks
{
    string _SourceCode;

    public HashBenchmarks()
    {

        static string GetRandomString(int stringLength)
        {
            StringBuilder sb = new StringBuilder();
            int numGuidsToConcat = (((stringLength - 1) / 32) + 1);
            for (int i = 1; i <= numGuidsToConcat; i++)
            {
                sb.Append(Guid.NewGuid().ToString("N"));
            }

            return sb.ToString(0, stringLength);
        }

        _SourceCode = GetRandomString(500_000);

    }



    [Benchmark]
    public void XXHash32()
    {
        var bytes = Encoding.UTF8.GetBytes(_SourceCode);
        for (int i = 0; i <= 1000; i++)
            _ = Extensions.Data.XXHash.XXH32(bytes);
    }



    [Benchmark]
    public void K4osXXHash32()
    {
        var bytes = Encoding.UTF8.GetBytes(_SourceCode);
        for (int i = 0; i <= 1000; i++)
        {
            _ = XXH32.DigestOf(bytes, 0, bytes.Length);
        }
    }

    [Benchmark]
    public void HashDepotXXHash32()
    {
        var bytes = Encoding.UTF8.GetBytes(_SourceCode);
        for (int i = 0; i <= 1000; i++)
        {
            _ = HashDepot.XXHash.Hash32(bytes);
        }
    }

    public void ConsoleOutput()
    {

        Console.WriteLine("32 " + Extensions.Data.XXHash.XXH32(Encoding.UTF8.GetBytes(_SourceCode)).ToString());
        Console.WriteLine("K4os32 " + Extensions.Data.XXHash.XXH32(Encoding.UTF8.GetBytes(_SourceCode)).ToString());
        Console.WriteLine("HashDepot32 " + HashDepot.XXHash.Hash32(Encoding.UTF8.GetBytes(_SourceCode)).ToString());
    }

}
