

namespace DeepSigma.AppInstallerManager.Models;

/// <summary>
/// Represents an application version with Major, Minor, Build, and Patch components.
/// </summary>
/// <param name="Major">Major version.</param>
/// <param name="Minor">Minor version</param>
/// <param name="Build">Build version.</param>
/// <param name="Patch">Patch version.</param>
public record class ApplicationVersion(int Major, int Minor, int Build, int Patch)
{
    /// <summary>
    /// Determines if this version is greater than the specified other version.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool IsGreaterThan(ApplicationVersion other)
    {
        if (Major > other.Major) return true;
        if (Major < other.Major) return false;

        if (Minor > other.Minor) return true;
        if (Minor < other.Minor) return false;

        if (Build > other.Build) return true;
        if (Build < other.Build) return false;

        if (Patch > other.Patch) return true;
        if (Patch < other.Patch) return false;
        return false;
    }

    /// <summary>
    /// Determines if this version is equal to the specified other version.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool IsEqualTo(ApplicationVersion other)
    {
        return this == other;
    }
}
