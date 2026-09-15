# 01-upgrade-domain-and-repository: Upgrade domain & repository libraries

Upgrade the class library projects to SDK-style csproj files and set TargetFramework to net10.0. Replace or update incompatible/deprecated NuGet packages. Rebuild and fix compile errors. Focus projects:
- src\A4U3.Domain\A4U3.Domain.csproj
- src\A4U3.Repository\A4U3.Repository.csproj

**Done when**: Both projects build without errors or new warnings and packages are compatible with net10.0.
