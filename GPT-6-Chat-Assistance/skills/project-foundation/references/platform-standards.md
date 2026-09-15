# Platform Standards

These standards govern the framework versions, language versions, and compilation settings for all projects in the MyProlink solution.

## Target Framework
- **Primary Framework**: `.NET 10.0` (`net10.0`)
- **Reason**: LTS release. No legacy `.NET Framework` or `.NET 8` projects are permitted.

## Language Version
- **C# Version**: `C# 12`
- **Nullable Reference Types**: `<Nullable>enable</Nullable>` must be set in all projects (typically enforced via `Directory.Build.props`).
- **Implicit Usings**: `<ImplicitUsings>enable</ImplicitUsings>` is permitted.

## Project File Configuration
Ensure the following properties are standard across all `.csproj` files unless overridden by `Directory.Build.props`:

```xml
<PropertyGroup>
  <TargetFramework>net10.0</TargetFramework>
  <ImplicitUsings>enable</ImplicitUsings>
  <Nullable>enable</Nullable>
</PropertyGroup>
```

## IDE & Tooling
- **Formatting**: Enforced via `.editorconfig`.
- **Linting**: StyleCop Analyzers must be used.
