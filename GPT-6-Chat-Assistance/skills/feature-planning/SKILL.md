---
name: feature-planning
description: Orchestrator skill used before implementing any new feature, bug fix, or refactor. Analyzes requirements, determines impacted layers, and dictates which specialized governance skills to load.
---

## Purpose
Acts as the central orchestrator to decompose feature requirements into a structured execution plan. Ensures that the correct, specialized agent skills are loaded based on the feature's architectural footprint.

## Feature Implementation Workflow
Before writing any code, execute this 6-step planning sequence:

1. **Analyze Requirements**: Parse the user's request to understand the business goal.
2. **Determine Impacted Layers**: Identify if the change hits the API, Service, Repository, Database, or Infrastructure layers.
3. **Determine Required Skills**: Cross-reference the impacted layers with the `Skill Dependency Matrix` (see below) to determine which governance skills govern those layers.
4. **Load Required Skills**: Explicitly read and load the required `SKILL.md` files into context.
5. **Generate Implementation Plan**: Draft a step-by-step checklist for the implementation. *(See `assets/implementation-plan-template.md`)*
6. **Execute Implementation**: Begin writing code, strictly adhering to the loaded skills and validation gates.

---

## Skill Dependency Matrix

Consult this matrix to determine which skills to load based on the feature type:

### New API Endpoint
- `project-foundation`
- `architecture-governance`
- `api-design-standards`
- `security-governance`
- `testing-and-quality`

### Database Change
- `project-foundation`
- `architecture-governance`
- `database-migrations`
- `efcore-database-conventions`
- `testing-and-quality`

### Authentication Feature
- `project-foundation`
- `architecture-governance`
- `security-governance`
- `api-design-standards`
- `testing-and-quality`

### External Integration
- `project-foundation`
- `architecture-governance`
- `integration-architecture`
- `security-governance`
- `observability-and-diagnostics`
- `testing-and-quality`

### Background Job
- `project-foundation`
- `architecture-governance`
- `background-job-processing`
- `observability-and-diagnostics`
- `testing-and-quality`

### Performance Optimization
- `performance-and-scalability`
- `observability-and-diagnostics`
- `testing-and-quality`

---

## References & Assets
- `assets/implementation-plan-template.md`
