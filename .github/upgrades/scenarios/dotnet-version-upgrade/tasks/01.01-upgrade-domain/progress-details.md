# Progress details — 01.01-upgrade-domain

Actions performed:

- Updated src\A4U3.Domain\A4U3.Domain.csproj TargetFramework -> net10.0
- Updated EF and Identity PackageReference versions to 8.0.0
- Fixed using directive in src\A4U3.Domain\Models\ApplicationUser.cs (Identity namespace)
- Restored and built the project successfully (warnings only; several packages flagged with known vulnerabilities that will be reviewed later)

Build result: Success (warnings)

Files modified:
- src\A4U3.Domain\A4U3.Domain.csproj
- src\A4U3.Domain\Models\ApplicationUser.cs

Notes:
- Repository project still references A4U3.EFContext which targets net452; repository build will be attempted after EFContext upgrade.
