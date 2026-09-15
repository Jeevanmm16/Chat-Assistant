---
name: deployment-and-release
description: Defines CI/CD pipelines, Azure deployment processes, environment management, release workflows, and operational readiness checks. Use when modifying infrastructure or deployment configurations.
---

## Purpose
Standardize deployment workflows and environment configuration.

## 1. CI/CD Pipelines
The project uses Azure DevOps (or GitHub Actions).
- **Build Phase**: Must restore dependencies, build the solution, run all unit tests, and publish the artifact.
- **Deploy Phase**: Must swap slots (if using App Services) to ensure zero-downtime deployments.

*(See `assets/azure-pipeline-template.yml`)*

## 2. Environment Management
- Development: `appsettings.Development.json`
- Staging/UAT: `appsettings.Staging.json`
- Production: `appsettings.Production.json`

## 3. Configuration Overrides
Do not commit sensitive configuration values to `.json` files. Use Azure App Service Application Settings (Environment Variables) or Azure Key Vault to override settings during deployment.

---

## References & Assets

- `references/azure-deployment.md`
- `assets/azure-pipeline-template.yml`
