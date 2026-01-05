
using DeepSigma.AppInstallerManager.Models;

namespace DeepSigma.OperatingSystem;

/// <summary>
/// A class to manage application installation and updates through the AppInstaller program.
/// </summary>
public static class AppInstaller
{
    /// <summary>
    /// Installs or updates the application if a newer version is available.
    /// </summary>
    /// <param name="installation_directory"></param>
    /// <param name="current_version"></param>
    /// <param name="app_name"></param>
    /// <param name="source_directory"></param>
    /// <param name="target_install_directory"></param>
    /// <param name="cli_source_directory"></param>
    /// <param name="auto"></param>
    public static void Install(string installation_directory, ApplicationVersion current_version, string app_name, string source_directory, string target_install_directory, string cli_source_directory, bool auto = true)
    {
        ApplicationVersion? latest_version = AppVersioningService.GetLatestApplicationVersionFromDirectory(installation_directory);

        if (latest_version is null) return; 
        if(latest_version.IsGreaterThan(current_version) == false) return;

        Terminal.RunCommand("AppInstaller", $"--app={app_name} --source={source_directory} --target={target_install_directory} --clisource={cli_source_directory} --auto={auto}");
        Environment.Exit(0); // Close this app after initiating the app installer program through terminal.
    }
}
