# scripts/verify-jobs.ps1
Write-Host "Verifying Background Job Standards..." -ForegroundColor Cyan

$hasErrors = $false
$srcFiles = Get-ChildItem -Path "src" -Filter "*Controller.cs" -Recurse -ErrorAction SilentlyContinue

foreach ($file in $srcFiles) {
    $content = Get-Content $file.FullName
    $text = $content -join "`n"

    # Detect Task.Run inside Controllers
    if ($text -match 'Task\.Run\(') {
        Write-Error "Job Violation: '$($file.Name)' uses Task.Run() inside a Controller. Use Hangfire BackgroundJob.Enqueue() for fire-and-forget tasks."
        $hasErrors = $true
    }
}

if ($hasErrors) {
    Write-Host "❌ Gate 1 Failed: Task.Run usage detected in Controllers." -ForegroundColor Red
    exit 1
}

Write-Host "✅ Gate 1 Passed: Background job standards enforced." -ForegroundColor Green
exit 0
