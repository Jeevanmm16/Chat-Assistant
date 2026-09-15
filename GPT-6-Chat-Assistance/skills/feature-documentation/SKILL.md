---
name: feature-documentation
description: Generates comprehensive technical documentation for a software feature or business capability by analyzing its project entry points, implementation flow, business logic, specifications, database interactions, external integrations, events, tests, and relevant history. Use when the user asks to document, explain, trace, or understand an entire feature end-to-end, such as order management, booking, authentication, payment processing, or user onboarding.
---

# Feature Documentation

## 1. Purpose

Document a software feature as an end-to-end business and technical capability. 
The goal is to explain what the feature does, why it exists, where it starts, how the request moves through the system, what business rules apply, and how it satisfies specifications using the project's knowledge graph.

## 2. When to Use

Use this skill when the user's request concerns an entire feature, business capability, or end-to-end workflow crossing multiple layers (API → Controller → Service → Repo → Database).

Examples:
* "Explain the booking management feature."
* "Document the order creation flow."
* "How does payment processing work in this project?"
* "Show me everything involved in order cancellation."

## 3. When NOT to Use

Do not use this skill when the requested scope is primarily a single implementation artifact.
Prefer narrower skills:
* Single method → `method-documentation`
* Single class → `class-documentation`
* Single API endpoint → `api-documentation`
* Entire project → `project-documentation`

## 4. Scope Boundary

The boundary is the business capability and its complete implementation flow.
This includes: Multiple endpoints, controllers, services, repositories, domain entities, database tables, external services, events, tests, and specifications.
Do not expand into unrelated project areas merely because they are reachable through the dependency graph.

## 5. How to Reason (Behavior)

Follow this execution sequence to document the feature:

1. **Identify Feature**: Resolve the feature name, specifications, and primary entry points.
2. **Resolve Entry Points**: Identify all meaningful endpoints or triggers (e.g., HTTP routes, events).
3. **Trace Graph**: Trace the execution flow down the dependency graph (Controller → Service → Repository → DB).
4. **Identify Business Logic**: Separate business rules from technical implementation details.
5. **Correlate Specification**: Compare the observed business logic with the specification requirements. Read `references/specification-correlation.md` for detailed correlation logic.
6. **Identify Dependencies**: Locate affected database tables, external integrations, background processes, and tests.
7. **Identify Gaps**: Highlight where implementation or specification gaps exist.

## 6. Evidence Rules

Every significant factual statement must be supported by project evidence (Code, Spec, Test, DB, Git). 
Never present an inferred or unknown behavior as an observed fact. 

For detailed rules on how to classify and trace evidence, read `references/evidence-rules.md`.

## 7. Output Requirements

Generate feature documentation covering: Overview, Specification, API surface, Execution flow, Business logic, Persistence, Integrations, Asynchronous processing, Tests, Traceability, History, and Gaps.

For the detailed and required output format, read `references/output-structure.md`. Ensure that your output adheres to the required `schema.json` structure.
