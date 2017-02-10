#addin nuget:?package=NuGet.Core&version=2.12.0
#addin "Cake.ExtendedNuGet"

var MyGetKey = EnvironmentVariable("MYGET_KEY");
string BuildNumber = EnvironmentVariable("TRAVIS_BUILD_NUMBER");
string Branch = EnvironmentVariable("TRAVIS_BRANCH");

Task("Restore")
    .Does(() =>
{
    var settings = new DotNetCoreRestoreSettings
    {
        Sources = new[] { "https://www.myget.org/F/discord-net/api/v2", "https://www.nuget.org/api/v2" }
    };
    DotNetCoreRestore(settings);
});
Task("CodeGen")
    .Does(() =>
{
    using (var process = StartAndReturnProcess("luajit", new ProcessSettings { Arguments = "generate.lua" }))
    {
        process.WaitForExit();
        var code = process.GetExitCode();
        if (code != 0)
        {
            throw new Exception(string.Format("Code Generation script failed! Exited {0}", code));
        }
    }
});
Task("Build")
    .Does(() =>
{
    var suffix = BuildNumber.PadLeft(5,'0');
    var settings = new DotNetCorePackSettings
    {
        Configuration = "Release",
        OutputDirectory = "./artifacts/",
        VersionSuffix = suffix
    };
    DotNetCorePack("./src/Discord.Addons.EmojiTools/", settings);
});
Task("Test")
    .Does(() =>
{
    DotNetCoreTest("./test/");
});
Task("Deploy")
    .WithCriteria(Branch == "master")
    .Does(() =>
{
    var settings = new NuGetPushSettings
    {
        Source = "https://www.myget.org/F/discord-net/api/v2/package",
        ApiKey = MyGetKey
    };
    var packages = GetFiles("./artifacts/*.nupkg");
    NuGetPush(packages, settings);
});

Task("Default")
    .IsDependentOn("Restore")
    .IsDependentOn("CodeGen")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Deploy")
    .Does(() => 
{
    Information("Build Succeeded");
});

RunTarget("Default");
