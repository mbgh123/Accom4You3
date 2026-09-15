# 02-upgrade-efcontext: Upgrade EFContext (data access)

Analyze EF usage: determine if EF6 code-first, EDMX, or LINQ-to-SQL is in use. If EF6 code-first, plan migration to EF Core; otherwise, update packages and APIs to supported equivalents. Update csproj to net10.0 and replace EF packages as needed. Run data access tests or smoke checks.

**Done when**: Data access project builds, unit/data tests pass, and migrations (if applied) are validated.
