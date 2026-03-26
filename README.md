# Dotnet.DeepSigma.AppInstallerManager

A focused .NET library for checking versioned deployment folders and handing off updates to an external `AppInstaller` process.

This package is designed for production applications that use a companion installer/updater workflow and want a lightweight way to:

- detect the latest available deployed version
- compare it to the currently running version
- launch an external installer when an update is available

## What it does

The library exposes three main pieces:

- `ApplicationVersion` — a simple version model with `Major`, `Minor`, `Build`, and `Patch`
- `AppVersioningService` — scans a deployment directory and returns the highest valid version it can parse
- `AppInstaller` — launches the `AppInstaller` command with the expected arguments and then exits the current process

Based on the source, the package targets `.NET 10.0`, is packaged as `DeepSigma.AppInstallerManager`, and depends on `DeepSigma.OperatingSystem` for terminal and file system interaction.

## Installation

The repository is configured to generate a NuGet package on build, but the repository page does not currently show a published package listing or releases. Use one of these approaches:

### Option 1: Reference the project directly

Add the project to your solution and reference it from your application.

### Option 2: Build and pack locally

```bash
dotnet pack DeepSigma.AppInstallerManager/DeepSigma.AppInstallerManager.csproj -c Release
```

Then consume the generated package from your local feed or internal package source.

## Target framework

- `.NET 10.0`

## Public API overview

### `ApplicationVersion`

Represents a version with four numeric parts:

```csharp
using DeepSigma.AppInstallerManager.Models;

var current = new ApplicationVersion(1, 0, 0, 0);
var latest  = new ApplicationVersion(1, 1, 0, 0);

bool isNewer = latest.IsGreaterThan(current);
bool same    = latest.IsEqualTo(current);
```

The comparison logic checks `Major`, then `Minor`, then `Build`, then `Patch`.

### `AppVersioningService`

Searches a deployment directory and returns the highest valid version found from subdirectory names.

Expected directory naming format:

```text
Major_Minor_Build_Patch
```

Examples of valid folder names:

```text
1_0_0_0
1_2_5_14
11_11_15_110
```

Invalid folder names are ignored. If the directory does not exist or no valid version folders are found, the method returns `null`.

Example:

```csharp
using DeepSigma.OperatingSystem;
using DeepSigma.AppInstallerManager.Models;

ApplicationVersion? latest =
    AppVersioningService.GetLatestApplicationVersionFromDirectory(
        @"C:\Deployments\MyApp"
    );

if (latest is not null)
{
    Console.WriteLine($"{latest.Major}.{latest.Minor}.{latest.Build}.{latest.Patch}");
}
```

### `AppInstaller`

Checks the deployment directory for the latest version. If a newer version than `current_version` exists, it runs:

```text
AppInstaller --app=... --source=... --target=... --clisource=... --auto=...
```

and then immediately exits the current application with `Environment.Exit(0)`. If no newer version is found, it does nothing.

Example:

```csharp
using DeepSigma.OperatingSystem;
using DeepSigma.AppInstallerManager.Models;

var currentVersion = new ApplicationVersion(1, 0, 0, 0);

AppInstaller.Install(
    installation_directory: @"C:\Deployments\MyApp",
    current_version: currentVersion,
    app_name: "MyApp",
    source_directory: @"C:\Deployments\MyApp\Packages",
    target_install_directory: @"C:\Program Files\MyApp",
    cli_source_directory: @"C:\Deployments\MyApp\Cli",
    auto: true
);
```

## How version discovery works

`GetLatestApplicationVersionFromDirectory(...)`:

1. Verifies the deployment directory exists
2. Enumerates subdirectories
3. Parses folder names split by `_`
4. Keeps only names with exactly four integer components
5. Calculates the highest `Major`, then `Minor`, then `Build`, then `Patch`
6. Returns a new `ApplicationVersion` for the maximum version found

This means the library expects version information to be encoded in folder names rather than file metadata or manifests.

## Behavioral notes

- The install flow is intentionally process-terminating after the external installer is launched. Design your application flow accordingly.
- Version parsing is strict: folder names must contain exactly four underscore-separated integer values.
- The library surface is intentionally small and appears meant to be embedded into apps that already follow a DeepSigma deployment/install convention. This is an inference from the repository structure and code layout.

## Project structure

```text
DeepSigma.AppInstallerManager/
├── DeepSigma.AppInstallerManager/
│   ├── Models/
│   │   └── ApplicationVersion.cs
│   ├── Utilities/
│   │   ├── AppInstaller.cs
│   │   └── AppVersioningService.cs
│   └── DeepSigma.AppInstallerManager.csproj
├── DeepSigma.AppInstallerManager.Test/
└── README.md
```

The repository also includes a separate test project targeting `.NET 10.0`.

## Suggested usage pattern

A typical app flow would look like this:

1. Build your application into versioned deployment folders such as `1_0_0_0`
2. Store the currently running app version as an `ApplicationVersion`
3. On startup or at a controlled checkpoint, call `AppInstaller.Install(...)`
4. Let the external installer take over only when a newer deployment exists

## Caveats

- The updater depends on an external `AppInstaller` command being available in the runtime environment.
- The library does not download updates by itself; it only discovers versions and launches the external installer.
- The repository does not currently expose release notes or published packages on the GitHub page.

## License

MIT.
