# 01.01-upgrade-domain: Upgrade domain project to net10.0

## Objective
Upgrade src\A4U3.Domain to net10.0, update EF and Identity package references, and fix compile errors.

## Steps
1. Ensure TargetFramework is net10.0 (already done).
2. Update package references to EF Core 8.0.0 and Identity packages.
3. Build and fix compile errors.

**Done when**: src\A4U3.Domain builds cleanly under net10.0.
