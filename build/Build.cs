using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
    public static int Main () => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath PublishDirectory => RootDirectory / "publish";

    AbsolutePath MainProjectFile => SourceDirectory / "XSharp.VsParser.Helpers" / "XSharp.VsParser.Helpers.csproj";


    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(PublishDirectory);
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution)
                .SetVerbosity(DotNetVerbosity.Quiet));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetNoRestore(true)
                .SetForce(true)
                .SetConfiguration(Configuration)
                .SetVerbosity(DotNetVerbosity.Quiet));
        });

    Target IncrementVersion => _ => _
        .Executes(() =>
        {
            VersionHelper.IncrementProjectVersion(MainProjectFile);
        });

    Target Publish => _ => _
        .DependsOn(Clean, Restore, IncrementVersion)
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetProject(MainProjectFile)
                .SetNoRestore(true)
                .SetForce(true)
                .SetContinuousIntegrationBuild(true)
                .SetConfiguration(Configuration.Release)
                .SetOutputDirectory(PublishDirectory)
                .SetVerbosity(DotNetVerbosity.Quiet));
        });
}
