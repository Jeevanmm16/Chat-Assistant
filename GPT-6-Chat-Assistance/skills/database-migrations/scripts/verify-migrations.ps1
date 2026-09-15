# scripts/verify-migrations.ps1
Write-Host "Verifying Database Migration Standards..." -ForegroundColor Cyan

$hasErrors = $false
$migrations = Get-ChildItem -Path "src" -Filter "*.cs" -Recurse | Where-Object { $_.FullName -match "Migrations" -and $_.Name -notmatch "Designer" -and $_.Name -notmatch "Snapshot" }

foreach ($migration in $migrations) {
    $content = Get-Content $migration.FullName
    $text = $content -join "`n"

    # Detect bad SQL execution inside migrations
    if ($text -match 'migrationBuilder\.Sql\("INSERT' -or $text -match 'migrationBuilder\.Sql\("UPDATE' -or $text -match 'migrationBuilder\.Sql\("DELETE') {
        Write-Error "Migration Violation: '$($migration.Name)' executes raw SQL strings directly. SQL must be stored in embedded .sql files and loaded via GetManifestResourceStream."
        $hasErrors = $true
    }
}

if ($hasErrors) {
    Write-Host "❌ Gate 1 Failed: Direct SQL execution detected in migrations." -ForegroundColor Red
    exit 1
}

Write-Host "✅ Gate 1 Passed: Migrations use embedded resources correctly." -ForegroundColor Green
exit 0
