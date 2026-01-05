using DeepSigma.OperatingSystem.FileSystem;
using DeepSigma.AppInstallerManager.Models;

namespace DeepSigma.OperatingSystem;

/// <summary>
/// Provides services related to application versioning, such as retrieving the latest version from a deployment directory.
/// </summary>
public static class AppVersioningService
{
    /// <summary>
    /// Returns the latest application version from the production app deplyment folder, if it exists. It does this by parsing the directory names. 
    /// </summary>
    /// <param name="production_deployment_directory"></param>
    /// <returns></returns>
    public static ApplicationVersion? GetLatestApplicationVersionFromDirectory(string production_deployment_directory)
    {
        if (Directory.Exists(production_deployment_directory) == false) return null;

        List<ApplicationVersion> versions = [];
        FileSystemObject fileSystem = new(production_deployment_directory, 5);
        string[] DirectoryNames = fileSystem.Directories.Select(x => x.DirectoryName).ToArray();
        foreach (string DirectoryName in DirectoryNames)
        {
            ApplicationVersion? version = GetApplicationVersionFromDirectoryName(DirectoryName);
            if (version is not null)
            {
                versions.Add(version);
            }
        }
        if (versions.Count == 0) return null;

        int MaximumMajor = versions.Select(x => x.Major).Max();
        int MaximumMinor = versions.Where(x => x.Major == MaximumMajor).Select(x => x.Minor).Max();
        int MaximumBuild = versions.Where(x => x.Major == MaximumMajor && x.Minor == MaximumMinor).Select(x => x.Build).Max();
        int maximumPatch = versions.Where(x => x.Major == MaximumMajor && x.Minor == MaximumMinor && x.Build == MaximumBuild).Select(x => x.Patch).Max();

        ApplicationVersion final_version = new(MaximumMajor, MaximumMinor, MaximumBuild, maximumPatch);
        return final_version;
    }

    /// <summary>
    /// Returns the app version if it can parse the values from the directory name. Returns null if it cannot.
    /// </summary>
    /// <param name="directory_name"></param>
    /// <returns></returns>
    private static ApplicationVersion? GetApplicationVersionFromDirectoryName(string directory_name)
    {
        string[] values = directory_name.Split('_');
        if(values.Length != 4) return null;

        bool MajorSuccess = Int32.TryParse(values[0], out int Major);
        bool MinorSuccess = Int32.TryParse(values[1], out int Minor);
        bool BuildSuccess = Int32.TryParse(values[2], out int Build);
        bool PatchSuccess = Int32.TryParse(values[3], out int Patch);

        return (MajorSuccess && MinorSuccess && BuildSuccess && PatchSuccess) 
            ? new ApplicationVersion(Major, Minor, Build, Patch)
            : null;
    }
}
