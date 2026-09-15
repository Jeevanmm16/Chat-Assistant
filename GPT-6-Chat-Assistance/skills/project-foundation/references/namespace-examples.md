# Namespace Rules & Examples

Namespaces must exactly match the folder structure relative to the `src/` directory. Use dots (`.`) for directory separators.

## Examples

**Correct Placement & Namespace:**
File: `src/<ProjectName>.Core.Application/Features/User/Commands/CreateUserCommand.cs`
Namespace:
```csharp
namespace <ProjectName>.Core.Application.Features.User.Commands;
```

**Correct Placement & Namespace:**
File: `src/<ProjectName>Core-API/Controllers/UserController.cs`
Namespace:
*(Note: Hyphens in project names are typically replaced with underscores in default namespaces).*
```csharp
namespace <ProjectName>Core_API.Controllers;
```

**❌ Incorrect Example:**
File: `src/<ProjectName>.Core.Domain/Entities/User.cs`
Namespace:
```csharp
namespace <ProjectName>.Core.Application.Entities; // ERROR: Does not match physical folder
```

**Rule Enforcement:**
The CI pipeline runs `dotnet format` which will automatically fix or flag namespace violations based on `.editorconfig` rules (`dotnet_style_namespace_match_folder = true`).
