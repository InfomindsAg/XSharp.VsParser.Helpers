using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Reports;

class Config : ManualConfig
{

    public Config()
    {
        AddLogger(DefaultConfig.Instance.GetLoggers().ToArray());
        AddColumnProvider(DefaultConfig.Instance.GetColumnProviders().ToArray());
        AddExporter(new CsvExporter(
            CsvSeparator.CurrentCulture,
            new SummaryStyle(
                System.Globalization.CultureInfo.CurrentCulture,
                true, //printUnitsInHeader
                SizeUnit.KB,
                Perfolizer.Horology.TimeUnit.Microsecond,
                false //printUnitsInContent
            )
        ));
    }
}