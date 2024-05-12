using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[GitHubActions("Build and test", GitHubActionsImage.Ubuntu2204,
    OnPushBranches = ["master", "feature/*"],
    OnPushExcludePaths = ["**.md"],
    InvokedTargets = [nameof(Test), nameof(VerifyStyle)],
    CacheKeyFiles = ["**/global.json", "**/*.csproj", "**/Directory.Build.props", "**/package-lock.json", ".nvmrc"],
    CacheIncludePatterns = [".nuke/temp", "~/.nuget/packages", "**/obj/*", "**/node_modules"],
    CacheExcludePatterns = new string[0])]
class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Compile);

    [Solution]
    readonly Solution Solution; 

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {

        });

    Target VerifyStyle => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetFormat(c => c
                .SetProcessWorkingDirectory(Solution.Directory)
                .EnableVerifyNoChanges()
                .EnableNoRestore()
                .SetVerbosity("diag"));
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(c => c
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(c => c
                .EnableNoRestore()
                .SetConfiguration(Configuration)
                .SetProjectFile(Solution)
                .SetProcessArgumentConfigurator(x => x
                    .Add("-warnaserror")
                    .Add("-warnnotaserror:CS0618,CS0612")));
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(c => c
                .EnableNoBuild()
                .SetConfiguration(Configuration)
                .SetProjectFile(Solution));
        });
}
