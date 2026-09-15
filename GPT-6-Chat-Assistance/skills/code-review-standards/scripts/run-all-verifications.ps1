# scripts/run-all-verifications.ps1
Write-Host "=============================================" -ForegroundColor Cyan
Write-Host "   RUNNING MASTER CODE REVIEW VERIFICATION   " -ForegroundColor Cyan
Write-Host "=============================================" -ForegroundColor Cyan

$hasGlobalErrors = $false

# Find all verification scripts in the skills directory
$verificationScripts = Get-ChildItem -Path "..\..\..\skills" -Filter "verify-*.ps1" -Recurse | Where-Object { $_.FullName -notmatch "run-all-verifications.ps1" }

if (-not $verificationScripts) {
    Write-Host "Warning: No verification scripts found in the skills directory." -ForegroundColor Yellow
    exit 0
}

foreach ($script in $verificationScripts) {
    Write-Host "`nExecuting: $($script.Name)" -ForegroundColor Magenta
    
    # Run the script
    try {
        $result = & $script.FullName
        if ($LASTEXITCODE -ne 0) {
            $hasGlobalErrors = $true
        }
    } catch {
        Write-Error "Failed to execute $($script.Name): $_"
        $hasGlobalErrors = $true
    }
}

Write-Host "`n=============================================" -ForegroundColor Cyan
if ($hasGlobalErrors) {
    Write-Host "❌ CODE REVIEW FAILED: One or more gates violated standards." -ForegroundColor Red
    exit 1
} else {
    Write-Host "✅ CODE REVIEW PASSED: All architectural and security gates are green." -ForegroundColor Green
    exit 0
}
