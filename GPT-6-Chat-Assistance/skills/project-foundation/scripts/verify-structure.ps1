# scripts/verify-structure.ps1
$requiredDirectories = @("src", "tests", "docs", "scripts", "build")
$missingDirectories = @()

foreach ($dir in $requiredDirectories) {
    if (-not (Test-Path -Path $dir -PathType Container)) {
        $missingDirectories += $dir
    }
}

if ($missingDirectories.Count -gt 0) {
    Write-Error "Gate 1 Failed: The following required directories are missing from the repository root:"
    foreach ($missing in $missingDirectories) {
        Write-Host "  - $missing" -ForegroundColor Red
    }
    Write-Host "Please create these directories and re-run the verification." -ForegroundColor Yellow
    exit 1
}

Write-Host "✅ Gate 1 Passed: All required directories exist." -ForegroundColor Green
exit 0
