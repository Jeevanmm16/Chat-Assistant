# Architecture Overview

The <ProjectName> solution follows an **N-Tier Architecture** pattern.

## Core Layers
1. **Entities (`src/<ProjectName>Core.Entities`)**
   - Contains database models and pure domain entities.
   - **Dependencies:** None.

2. **DTO (`src/<ProjectName>Core.DTO`)**
   - Contains Data Transfer Objects used to pass data between layers and return responses from the API.
   - **Dependencies:** None (or `Entities`).

3. **Repository (`src/<ProjectName>Core.Repository`)**
   - Implements data access logic (Entity Framework Core).
   - Contains repository interfaces and implementations.
   - **Dependencies:** `Entities`

4. **Service (`src/<ProjectName>Core.Service`)**
   - Contains core business logic.
   - Orchestrates calls between repositories and maps to DTOs.
   - **Dependencies:** `Repository`, `DTO`, `Entities`

5. **API (`src/<ProjectName>Core.API`)**
   - The entry point of the application.
   - Handles HTTP requests, routing, and dependency injection.
   - **Dependencies:** `Service`, `DTO`

6. **Utility (`src/<ProjectName>Core-Utility`)**
   - Azure Functions and background processing tasks.
