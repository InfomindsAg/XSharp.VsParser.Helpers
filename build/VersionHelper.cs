using Nuke.Common;
using Nuke.Common.IO;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

public static class VersionHelper
{
    static string IncrementVersionYearWeekRelease(string currentVersion)
    {
        var currentVersionParts = currentVersion?.Split('.', StringSplitOptions.RemoveEmptyEntries);
        Assert.True(currentVersionParts?.Length >= 3, "Invalid current version");
        Assert.True(int.TryParse(currentVersionParts[2], out var currentRelease), "Invalid current version release");

        var currentVersionInt = currentVersionParts.Select(q => int.Parse(q)).ToList();

        var now = DateTime.Now;
        var year = (ISOWeek.GetYear(now) - 2000);
        var week = (ISOWeek.GetWeekOfYear(now));

        Assert.True(currentVersionInt[0] < year || (currentVersionInt[0] == year && currentVersionInt[1] <= week), "Current version year/week is in the future");

        var result = new List<string> { year.ToString(), week.ToString() };
        if (currentVersionInt[0] == year && currentVersionInt[1] == week)
            result.Add((currentRelease + 1).ToString());
        else
            result.Add("0");

        for (int i = 4; i <= currentVersionParts.Length; i++)
            result.Add("0");

        return string.Join(".", result);
    }

    public static void IncrementProjectVersion(AbsolutePath projectFileName)
    {
        var xml = XDocument.Load(projectFileName);
        var version = xml.Root.Elements("PropertyGroup").Where(q => q.Element("Version") != null).Select(q => q.Element("Version").Value).FirstOrDefault();

        var newVersion = IncrementVersionYearWeekRelease(version ?? "1.0.0");

        var content = File.ReadAllText(projectFileName);
        Log.Information("Incrementing project version from {version} to {newVersion}", version, newVersion);
        content = content.Replace($"<Version>{version}</Version>", $"<Version>{newVersion}</Version>");
        File.WriteAllText(projectFileName, content);
    }
}
