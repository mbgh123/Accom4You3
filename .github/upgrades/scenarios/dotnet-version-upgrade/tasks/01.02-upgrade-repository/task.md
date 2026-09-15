# 01.02-upgrade-repository: Upgrade repository project to net10.0

## Objective
Upgrade src\A4U3.Repository to net10.0 after A4U3.EFContext is upgraded.

## Steps
1. Retarget to net10.0.
2. Ensure project references are compatible (A4U3.Domain, A4U3.EFContext).
3. Update packages and build.

**Done when**: src\A4U3.Repository builds under net10.0.
