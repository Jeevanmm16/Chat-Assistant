# scripts/verify-efcore-performance.ps1
Write-Host "Verifying EF Core Repository Standards..." -ForegroundColor Cyan

$hasErrors = $false

# Find Repositories
$repoFolders = Get-ChildItem -Path "src" -Directory -Recurse -Filter "Implementations" | Where-Object { $_.FullName -match "Repository" }

if ($repoFolders.Count -eq 0) {
    Write-Host "Warning: Repository folder not found." -ForegroundColor Yellow
    exit 0
}

foreach ($folder in $repoFolders) {
    $repos = Get-ChildItem -Path $folder.FullName -Filter "*Repository.cs" -Recurse

    foreach ($repo in $repos) {
        $content = Get-Content $repo.FullName
        $text = $content -join "`n"

        # Check for Premature ToList()
        # Regex looks for .ToList() or .ToListAsync() immediately followed by .Where() or .Select() on the same or next few lines
        if ($text -match '\.ToList(?:Async)?\(\)\s*\.Where') {
            Write-Error "EF Core Violation: '$($repo.Name)' contains premature execution (e.g. .ToList().Where()). Filter data in the DB before calling ToList()."
            $hasErrors = $true
        }
        
        # Check for IEnumerable return types instead of IQueryable
        if ($text -match 'IEnumerable<.*>\s*\w+\s*=\s*_context') {
             Write-Error "EF Core Violation: '$($repo.Name)' assigns DbContext output to IEnumerable. Use IQueryable to execute in SQL Server."
             $hasErrors = $true
        }
    }
}

if ($hasErrors) {
    Write-Host "❌ Gate 1 Failed: EF Core performance rules were violated." -ForegroundColor Red
    exit 1
}

Write-Host "✅ Gate 1 Passed: Repository logic adheres to EF Core standards." -ForegroundColor Green
exit 0
