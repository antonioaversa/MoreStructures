# More Structures .NET
More Structure .NET is a OS-agnostic library written in .NET6 and C# 10, targeting Any CPU.

## API Documentation
The API Documentation, generated from [XML documentation comments](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/), and compiled into a navigable website via [DocFX](https://dotnet.github.io/docfx/), is available [here](api/index.md).

## Prerequisites
- Windows, Linux or macOS
- Having .NET6 or later installed
  - check [here](https://docs.microsoft.com/en-us/dotnet/core/install/) for supported OS, macOS releases and Linux distributions, dependencies and installation instructions. 
- You can check whether and which .NET is installed in your system by running the following command
  ```
  dotnet --list-sdks
  dotnet --list-runtimes
  ```

## Build
The library can be built on Windows, Linux and macOS.

To build a Release version of the library, run the following commands in the directory containing `MoreStructures.sln`:
```
dotnet restore
dotnet build --no-restore --configuration Release
``` 

To build a Debug version of the library, run the following commands in the directory containing `MoreStructures.sln`:
```
dotnet restore
dotnet build --no-restore --configuration Debug
``` 

## Test
To run unit tests, after having built the project, run the following command in the directory containing `MoreStructures.sln`:
```
dotnet test --no-build --verbosity normal --configuration Debug
```

## Dependencies
The library is 100% C# and .NET Managed code, with no OS dependencies and with minimal nuget external dependencies, listed below. 

### MoreLINQ
More Structures depends on MoreLINQ, a battle-tested library used for many typical LINQ operations which are not out of the box in .NET.

Visit https://morelinq.github.io/ for further information.
