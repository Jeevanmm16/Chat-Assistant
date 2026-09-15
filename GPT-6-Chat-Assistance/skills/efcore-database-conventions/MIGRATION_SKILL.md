---
name: database-migrations
description: Create and maintain Entity Framework Core migrations following project standards. Use when creating or modifying EF Core database migrations, SQL scripts, or seed data.
---

## Purpose
Create and maintain Entity Framework Core migrations following `<ProjectName>Core` project standards. All migrations must execute SQL scripts from embedded resource files.

---

## Migration Workflow

1. **Create migration class** using EF Core CLI.
2. **Create matching SQL file** in the `SQL` folder (e.g., `20260312121831_Added_EmailTemplate.sql`).
3. **Mark SQL file as Embedded Resource** in the `.csproj` file. Verify `<EmbeddedResource Include="SQL\*.sql" />` exists.
4. **Reference SQL file inside migration** using `Assembly.GetManifestResourceStream`.
5. **Execute SQL** using `migrationBuilder.Sql(sql);`.

---

## Migration Class Standard

Do not place large SQL statements directly inside migration classes. Store SQL inside dedicated `.sql` files.

```csharp
public partial class Added_EmailTemplate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        var assembly = System.Reflection.Assembly.GetExecutingAssembly();
        var resourceName = "<ProjectName>Core.Entities.SQL.20260312121831_Added_EmailTemplate.sql";

        using (var stream = assembly.GetManifestResourceStream(resourceName))
        using (var reader = new StreamReader(stream))
        {
            string sql = reader.ReadToEnd();
            migrationBuilder.Sql(sql);
        }
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Only implement Down when rollback is explicitly required.
    }
}
```

---

## SQL Script Standards (Idempotency)

All insert scripts **must be safe to run multiple times** (Idempotent). 
**Always use `WHERE NOT EXISTS` for inserts.**

### Good Example (Safe for repeated deployments)
```sql
INSERT INTO emailtemplate (TemplateName, TemplateHtml, Subject)
SELECT 'Talent Failed to Start', @html, 'Talent Failed to Start'
WHERE NOT EXISTS (
    SELECT 1 FROM emailtemplate WHERE TemplateName = 'Talent Failed to Start'
);
```

### Bad Example (Creates duplicates)
```sql
INSERT INTO emailtemplate (TemplateName) VALUES ('New Template');
```

---

## Startup Migrations
- The project does **NOT** run `Database.Migrate();` during startup.
- Migrations are applied out-of-band by operations teams. 
- **Do not add automatic migration execution to `Program.cs`.**

---

## Gotchas

- **Incorrect Resource Name**: Verify that `assembly.GetManifestResourceStream(resourceName)` matches the actual namespace and file path. This is the most common issue.
- **SQL File Not Embedded**: If the Build Action is not set to `Embedded Resource`, the migration will fail at runtime.
- **Hardcoded SQL Inside Migration**: Avoid `migrationBuilder.Sql("INSERT ...");` for large scripts.

---

## References

- `references/migration-validation-checklist.md`
  Run before finalizing migration code.
