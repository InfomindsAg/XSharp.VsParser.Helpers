using FluentAssertions;
using Xunit;
using static XSharp.Parser.Helpers.Tests.TestHelpers.TestHelperExtensions;
using System.IO;
using System.Text;

namespace XSharp.Parser.Helpers.Tests.ClassHierarchy;

public class ClassHierarchyTests
{
    readonly XSharp.VsParser.Helpers.ClassHierarchy.ClassHierarchy _classHierarchy;

    public ClassHierarchyTests()
    {
        _classHierarchy = new XSharp.VsParser.Helpers.ClassHierarchy.ClassHierarchy();
        _classHierarchy.AnalyzeProject(TestProjectFileName());
    }

    [Fact]
    public void IsBaseClassTest()
    {
        _classHierarchy.IsBaseClass("A_A", "A").Should().BeTrue();
        _classHierarchy.IsBaseClass("A", "A_A").Should().BeFalse();
    }

    [Fact]
    public void GetProjectFileName()
    {
        _classHierarchy.GetProjectFileName("A_A").Should().Be("TestProject.xsproj");
    }

}