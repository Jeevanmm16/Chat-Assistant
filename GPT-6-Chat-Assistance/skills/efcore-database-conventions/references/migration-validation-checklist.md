# Migration Validation Checklist

Before returning migration code, verify the following:

- [ ] Migration class created.
- [ ] SQL file created in the `SQL/` folder.
- [ ] SQL file marked as `EmbeddedResource` in the `.csproj`.
- [ ] Resource name inside `GetManifestResourceStream` is exact and correct.
- [ ] SQL is stored outside the migration class.
- [ ] `WHERE NOT EXISTS` used for all `INSERT` statements to ensure idempotency.
- [ ] NO `Database.Migrate()` added to application startup.
- [ ] Migration compiles successfully.
