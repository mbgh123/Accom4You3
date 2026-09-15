# Upgrade A4U3B.sln to .NET 10

## Overview

**Target**: Upgrade all projects in A4U3B.sln to target net10.0
**Scope**: Solution with multiple projects currently targeting .NET Framework 4.5.2; key projects include domain, repository, EFContext, web, and several test projects.

## Tasks

### 01-upgrade-domain-and-repository: Upgrade domain & repository libraries

Upgrade the class library projects to SDK-style csproj files and set TargetFramework to net10.0. Replace or update incompatible/deprecated NuGet packages. Rebuild and fix compile errors. Focus projects:
- src\A4U3.Domain\A4U3.Domain.csproj
- src\A4U3.Repository\A4U3.Repository.csproj

**Done when**: Both projects build without errors or new warnings and packages are compatible with net10.0.

---

### 02-upgrade-efcontext: Upgrade EFContext (data access)

Analyze EF usage: determine if EF6 code-first, EDMX, or LINQ-to-SQL is in use. If EF6 code-first, plan migration to EF Core; otherwise, update packages and APIs to supported equivalents. Update csproj to net10.0 and replace EF packages as needed. Run data access tests or smoke checks.

**Done when**: Data access project builds, unit/data tests pass, and migrations (if applied) are validated.

---

### 03-upgrade-web-app: Upgrade web project to net10.0

Migrate web project to SDK-style csproj and update TargetFramework to net10.0. Replace System.Web-dependent patterns with ASP.NET Core alternatives if necessary. Update middleware, authentication, session, and hosting code per net10.0 runtime.

**Done when**: Web project builds and a local smoke run starts without critical errors.

---

### 04-update-tests: Upgrade test projects

Update test projects to target net10.0 and upgrade test framework packages (e.g., NUnit/xUnit) to compatible versions. Run tests and fix API/behavioral change issues.

**Done when**: All test projects build and tests pass (or failures are documented and accepted).

---

### 05-integration-validation: Full-solution build and CI validation

Build the entire solution, run full test suite, and validate CI changes. Address remaining package and compatibility issues. Update CI config to use .NET 10 SDK if needed.

**Done when**: Solution builds cleanly in local CI and tests pass.

---

### 06-cleanup-and-documentation: Finalize changes

Document migration decisions, package replacements, any manual deployment steps, and update scenario artifacts.

**Done when**: Documentation is added and scenario is ready to complete.

