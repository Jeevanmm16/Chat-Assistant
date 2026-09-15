# scripts/verify-logging.ps1
Write-Host "Verifying Logging Standards..." -ForegroundColor Cyan

$hasErrors = $false
$srcFiles = Get-ChildItem -Path "src" -Filter "*.cs" -Recurse -ErrorAction SilentlyContinue

if (-not $srcFiles) {
    Write-Host "Warning: No C# files found in src/." -ForegroundColor Yellow
    exit 0
}

foreach ($file in $srcFiles) {
    $content = Get-Content $file.FullName
    $text = $content -join "`n"

    # Check for Console.WriteLine or Debug.WriteLine
    if ($text -match 'Console\.WriteLine\(' -or $text -match 'Debug\.WriteLine\(') {
        Write-Error "Logging Violation: '$($file.Name)' contains Console.WriteLine or Debug.WriteLine. Use _logger instead."
        $hasErrors = $true
    }

    # Rudimentary check for logging sensitive terms
    if ($text -match '(?i)_logger\.Log.*\(.*password.*\,|token|secret.*\)') {
        Write-Warning "Potential Security Violation: '$($file.Name)' appears to be logging a sensitive keyword (password, token, or secret). Please verify manually."
    }
}

if ($hasErrors) {
    Write-Host "❌ Gate 1 Failed: Forbidden logging methods detected." -ForegroundColor Red
    exit 1
}

Write-Host "✅ Gate 1 Passed: Logging standards are enforced." -ForegroundColor Green
exit 0
