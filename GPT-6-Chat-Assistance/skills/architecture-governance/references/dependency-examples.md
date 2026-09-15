# Dependency Examples

This file provides concrete examples of valid and invalid references across the layers of the <ProjectName> architecture.

## Valid References

**API Layer (`<ProjectName>Core.API`)**
```xml
<!-- ALLOWED -->
<ProjectReference Include="..\<ProjectName>Core.Service\<ProjectName>Core.Service.csproj" />
<ProjectReference Include="..\<ProjectName>Core.DTO\<ProjectName>Core.DTO.csproj" />
```

**Service Layer (`<ProjectName>Core.Service`)**
```xml
<!-- ALLOWED -->
<ProjectReference Include="..\<ProjectName>Core.Repository\<ProjectName>Core.Repository.csproj" />
<ProjectReference Include="..\<ProjectName>Core.DTO\<ProjectName>Core.DTO.csproj" />
<ProjectReference Include="..\<ProjectName>Core.Entities\<ProjectName>Core.Entities.csproj" />
```

**Repository Layer (`<ProjectName>Core.Repository`)**
```xml
<!-- ALLOWED -->
<ProjectReference Include="..\<ProjectName>Core.Entities\<ProjectName>Core.Entities.csproj" />
```

## Invalid References (Forbidden)

**DTO Layer (`<ProjectName>Core.DTO`)**
```xml
<!-- FORBIDDEN: DTO must never reference Entities -->
<ProjectReference Include="..\<ProjectName>Core.Entities\<ProjectName>Core.Entities.csproj" />
```

**Entities Layer (`<ProjectName>Core.Entities`)**
```xml
<!-- FORBIDDEN: Entities must be completely pure, no references allowed -->
<ProjectReference Include="..\<ProjectName>Core.Service\<ProjectName>Core.Service.csproj" />
```

**Repository Layer (`<ProjectName>Core.Repository`)**
```xml
<!-- FORBIDDEN: Repository must never know about Service or API -->
<ProjectReference Include="..\<ProjectName>Core.Service\<ProjectName>Core.Service.csproj" />
<ProjectReference Include="..\<ProjectName>Core.API\<ProjectName>Core.API.csproj" />
```
