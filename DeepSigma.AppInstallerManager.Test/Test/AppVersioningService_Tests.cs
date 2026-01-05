using Xunit;
using DeepSigma.AppInstallerManager.Test;
using DeepSigma.AppInstallerManager.Models;

namespace DeepSigma.OperatingSystem.Tests.Tests;

public class AppVersioningService_Tests
{
    [Fact]
    public void GetLatestApplicationVersionFromDirectory_ShouldReturnNullForNonExistentDirectory()
    {
        string nonExistentDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        ApplicationVersion? result = AppVersioningService.GetLatestApplicationVersionFromDirectory(nonExistentDirectory);
        Assert.Null(result);
    }

    [Fact]
    public void GetLatestApplicationVersionFromDirectory_ShouldReturnValue()
    {
        ApplicationVersion? result = AppVersioningService.GetLatestApplicationVersionFromDirectory(TestConfig.GetTestVersionDirectoryPath());
        Assert.NotNull(result);
    }

    [Fact]
    public void GetLatestApplicationVersionFromDirectory_ShouldMatchExpectedValue()
    {
        ApplicationVersion? actual = AppVersioningService.GetLatestApplicationVersionFromDirectory(TestConfig.GetTestVersionDirectoryPath());
        ApplicationVersion expected = new(11, 11, 15, 110);
        Assert.Equal(expected, actual);
    }
}
