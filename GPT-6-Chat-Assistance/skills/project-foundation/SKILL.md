---
name: project-foundation
description: Enforce rigid project structure, naming conventions, and file placement for the .NET 10 N-Tier architecture. Use when creating files, projects, or namespaces.
---

## Purpose

Ensure all code generation strictly adheres to the <ProjectName> solution standards to:
- Maintain consistent dependency boundaries (Clean Architecture).
- Ensure CI/CD validation gates succeed unconditionally.

---

## Canonical Layout

```text
<ProjectName>/
├── src/
│   ├── <ProjectName>Core.API/          ← ASP.NET Web API host
│   ├── <ProjectName>Core.Service/      ← Business logic
│   ├── <ProjectName>Core.Repository/   ← Data access (EF Core)
│   ├── <ProjectName>Core.DTO/          ← Data transfer objects
│   ├── <ProjectName>Core.Entities/     ← DB models and entities
│   └── <ProjectName>Core-Utility/      ← Azure Functions
├── tests/                          ← all test projects (plural)
├── docs/
├── scripts/
└── build/                          ← Directory.Build.props, CI configs
```

- **Full tree with exact sub-folders** → `references/folder-structure.txt`
- **Architecture overview** → `references/architecture-overview.md` (or `assets/architecture-diagram.png`)

---

## Placement Decision

When creating code, use these rules to determine the correct location:

- **Database Model / Entity?** → `src/<ProjectName>Core.Entities/`
- **Data Transfer Object (DTO)?** → `src/<ProjectName>Core.DTO/`
- **Data Access / EF Core?** → `src/<ProjectName>Core.Repository/`
- **Business Logic / Orchestration?** → `src/<ProjectName>Core.Service/`
- **Controller / Endpoint?** → `src/<ProjectName>Core.API/`
- **Azure Function?** → `src/<ProjectName>Core-Utility/`
- **Unit Tests?** → `tests/`
- **Documentation?** → `docs/`
- **Scripts / Tooling?** → `scripts/`

---

## Code Organization

Code within each layer should be organized by feature or domain concept, not purely by technical concern where possible, to prevent massive folders. For example:

```text
src/<ProjectName>Core.Service/
    ├── User/
    │   ├── IUserService.cs
    │   └── UserService.cs
    └── Timecard/
        ├── ITimecardService.cs
        └── TimecardService.cs
```

---

## Dependency Rules

To maintain the N-Tier Architecture, project references MUST follow these strict rules.

**✅ Allowed:**
- API → Service, DTO
- Service → Repository, DTO, Entities
- Repository → Entities
- DTO → Entities (Optional)

**❌ Not Allowed:**
- Entities → Anything (Entities must remain pure)
- DTO → Service, Repository, API
- Repository → Service, API
- Service → API

---

## Rules

| ID     | Rule                                                                                      | Enforced by        |
|--------|-------------------------------------------------------------------------------------------|--------------------|
| PF-001 | Root must contain exactly: `src/`, `tests/`, `docs/`, `scripts/`, `build/`.              | `verify-structure.ps1` |
| PF-002 | Namespace = folder path relative to `src/`, dots as separators. See `references/namespace-examples.md`. | `dotnet format` CI |
| PF-003 | All NuGet versions declared in `build/Directory.Build.props`. No inline `Version=` in `.csproj`. | CI scan      |
| PF-004 | No wildcard `using` statements.                                                           | StyleCop / linter  |

*(Note: Target framework and language versions are defined in `references/platform-standards.md`)*

---



## Test Project Naming

Test projects in the `tests/` directory must mirror the source project name with `.Tests` appended:
- `<ProjectName>Core.API` → `<ProjectName>Core.API.Tests`
- `<ProjectName>Core.Service` → `<ProjectName>Core.Service.Tests`

---

## Workflow

### A — Initialize Repository

```
1. Clone repo.
2. Run: scripts/setup.ps1
3. Go to Gate 1.
```

### B — Gate 1 · Folder Layout

```powershell
scripts/verify-structure.ps1
```

- ✅ Pass → proceed to Gate 2.
- ❌ Fail → create the directories listed in the script output, then re-run Gate 1.

### C — Gate 2 · Namespace & Format

```powershell
dotnet format --verify-no-changes
```

- ✅ Pass → code is ready to commit.
- ❌ Fail → run `dotnet format` (no flag) to auto-fix, commit fixes, re-run Gate 2.

### D — Add a New Project

Follow `references/add-new-project.md` (covers: create `.csproj`, update `<ProjectName>.sln`, add version block to `build/Directory.Build.props`). Load `references/platform-standards.md` for target framework rules.

### E — Commit & Branch

Follow `references/branch-commit-conventions.md`.

---

## Gotchas

- **Solution file not updated** — adding a `.csproj` without adding it to `<ProjectName>.sln` causes CI to silently skip that project's tests.
- **Inline NuGet version** — any `Version=` attribute left in a `.csproj` is overridden to empty on CI; always declare the version in `build/Directory.Build.props` first.
- **Namespace–folder mismatch** — CI blocks merge; run `dotnet format` locally before pushing.
- **`tests/` spelling** — root folder is `tests/` (plural). Using `test/` (singular) breaks `verify-structure.ps1`.
- **New package not in central props** — referencing a NuGet package without adding its version to `build/Directory.Build.props` will produce a restore error in CI but may succeed locally if the package is cached.
- **AI Layer Misplacement** — AI agents frequently try to place business logic in the API layer or data access in the Service layer. Keep them strictly separated according to the Dependency Rules.
- **Secrets in source** — never store secrets in `appsettings.json`; use environment variables or Azure Key Vault. See `authentication-authorization` skill.
