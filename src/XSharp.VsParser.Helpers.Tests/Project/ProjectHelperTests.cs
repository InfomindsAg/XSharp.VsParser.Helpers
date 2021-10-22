using FluentAssertions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using XSharp.VsParser.Helpers.Project;
using Xunit;
using static XSharp.Parser.Helpers.Tests.TestHelpers.TestHelperExtensions;

namespace XSharp.Parser.Helpers.Tests.Project
{
    public class ProjectHelperTests
    {
        static readonly List<string> AllVoFlags = new()
        {
            "az",
            "cs",
            "lb",
            "ovf",
            "unsafe",
            "ins",
            "ns",
            "vo1",
            "vo2",
            "vo3",
            "vo4",
            "vo5",
            "vo6",
            "vo7",
            "vo8",
            "vo9",
            "vo10",
            "vo11",
            "vo12",
            "vo13",
            "vo14",
            "vo15",
            "vo16",
        };

        static List<string> AllFlags(List<string> flags, bool value)
            => flags.Select(q => $"{q}{(value ? "+" : "-")}").ToList();

        [Fact]
        public void VoAllFlagsTrue()
        {
            var projectHelper = new ProjectHelper(ProjectFile("XSharpVoAllFlagsTrue.xsproj"));
            var options = projectHelper.GetOptions();

            options.Should().Contain(AllFlags(AllVoFlags, true));
        }

        [Fact]
        public void VoAllFlagsFalse()
        {
            var projectHelper = new ProjectHelper(ProjectFile("XSharpVoAllFlagsFalse.xsproj"));
            var options = projectHelper.GetOptions();

            options.Should().Contain(AllFlags(AllVoFlags, false));
        }

        [Fact]
        public void GetSourceFiles()
        {
            var projectHelper = new ProjectHelper(CodeFile("XSharpExamples.xsproj"));
            var sourceFiles = projectHelper.GetSourceFiles();

            sourceFiles.Should().BeEquivalentTo("Program.prg", "StringBuilderExamples.prg");
        }

        [Fact]
        public void GetSourceFilesFullPath()
        {
            var fileName = CodeFile("XSharpExamples.xsproj");
            var path = Path.GetDirectoryName(fileName);
            var projectHelper = new ProjectHelper(fileName);
            var sourceFiles = projectHelper.GetSourceFiles(true);

            sourceFiles.Should().BeEquivalentTo(Path.Combine(path, "Program.prg"), Path.Combine(path, "StringBuilderExamples.prg"));
        }

    }
}
