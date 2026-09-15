# Add New Project Workflow

Follow these steps when creating a new .NET project in the <ProjectName> solution.

## 1. Determine Location and Type
Use the `Placement Decision` rules in `SKILL.md` to decide where the project goes.
- Example: `src/<ProjectName>.Core.Application`
- Example Test: `tests/<ProjectName>.Core.Application.Tests`

## 2. Create the Project
Run the `dotnet new` command from the root of the repository.
```powershell
# Example for a class library
dotnet new classlib -n <ProjectName>.NewProject -o src/<ProjectName>.NewProject

# Example for a test project
dotnet new xunit -n <ProjectName>.NewProject.Tests -o tests/<ProjectName>.NewProject.Tests
```

## 3. Add to Solution
You MUST add the new project to the `.sln` file so it builds in CI.
```powershell
dotnet sln <ProjectName>.sln add src/<ProjectName>.NewProject/<ProjectName>.NewProject.csproj
```

## 4. Add Project References
If the project depends on other internal projects, add them. Ensure you follow the **Dependency Rules** in `SKILL.md`.
```powershell
dotnet add src/<ProjectName>.NewProject/<ProjectName>.NewProject.csproj reference src/<ProjectName>.Core.Domain/<ProjectName>.Core.Domain.csproj
```

## 5. Handle NuGet Dependencies
If the project requires external NuGet packages:
1. Open `build/Directory.Build.props`
2. Add the `<PackageReference Update="PackageName" Version="x.y.z" />` in the central package management section.
3. In your `.csproj`, add the reference **without** a version:
   `<PackageReference Include="PackageName" />`

## 6. Validate
Run Gate 2 (`dotnet format --verify-no-changes`) and `dotnet build` to ensure everything is correct.
