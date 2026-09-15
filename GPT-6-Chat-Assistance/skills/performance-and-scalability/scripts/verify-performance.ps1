# scripts/verify-performance.ps1
Write-Host "Verifying Performance Standards..." -ForegroundColor Cyan

$hasErrors = $false
$srcFiles = Get-ChildItem -Path "src" -Filter "*.cs" -Recurse -ErrorAction SilentlyContinue

if (-not $srcFiles) {
    Write-Host "Warning: No C# files found in src/." -ForegroundColor Yellow
    exit 0
}

foreach ($file in $srcFiles) {
    $content = Get-Content $file.FullName
    $text = $content -join "`n"

    # Detect Synchronous Blocking (Thread Starvation)
    if ($text -match '\.Result\b' -or $text -match '\.Wait\(\)' -or $text -match '\.GetAwaiter\(\)\.GetResult\(\)') {
        # Exclude common test file patterns if they accidentally leaked into src, or Program.cs setups
        if ($file.Name -notmatch "Program.cs") {
            Write-Error "Performance Violation: '$($file.Name)' contains blocking calls (.Result, .Wait(), or .GetAwaiter().GetResult()). Use async/await instead to prevent thread starvation."
            $hasErrors = $true
        }
    }
}

if ($hasErrors) {
    Write-Host "❌ Gate 1 Failed: Synchronous blocking detected." -ForegroundColor Red
    exit 1
}

Write-Host "✅ Gate 1 Passed: No thread-blocking calls found." -ForegroundColor Green
exit 0
