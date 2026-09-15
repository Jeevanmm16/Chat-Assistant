# scripts/verify-httpclient.ps1
Write-Host "Verifying HttpClient Usage..." -ForegroundColor Cyan

$hasErrors = $false
$srcFiles = Get-ChildItem -Path "src" -Filter "*.cs" -Recurse -ErrorAction SilentlyContinue

foreach ($file in $srcFiles) {
    $content = Get-Content $file.FullName
    $text = $content -join "`n"

    # Detect bad instantiation
    if ($text -match 'new HttpClient\(') {
        Write-Error "Integration Violation: '$($file.Name)' instantiates HttpClient directly. Use IHttpClientFactory or Typed Clients."
        $hasErrors = $true
    }
}

if ($hasErrors) {
    Write-Host "❌ Gate 1 Failed: Direct HttpClient instantiation detected." -ForegroundColor Red
    exit 1
}

Write-Host "✅ Gate 1 Passed: Integration standards enforced." -ForegroundColor Green
exit 0
