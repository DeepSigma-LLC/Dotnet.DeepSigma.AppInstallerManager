
namespace DeepSigma.AppInstallerManager.Test;

internal static class TestConfig
{
    internal static string GetTestVersionDirectoryPath()
    {
        string full_directory_path = Path.Combine(GetTestDataDirectoryPath(), "TestDirectories_DoNotDelete");
        return full_directory_path;
    }

    internal static string GetTestDataDirectoryPath()
    {
        string base_directory = AppDomain.CurrentDomain.BaseDirectory;

        for (int i = 0; i <= 3; i++)
        {
            DirectoryInfo? parent_directory = Directory.GetParent(base_directory);
            if (parent_directory is null) break;
            base_directory = parent_directory.FullName;
        }

        string full_directory_path = Path.Combine(base_directory, "Data");
        return full_directory_path;
    }
}
