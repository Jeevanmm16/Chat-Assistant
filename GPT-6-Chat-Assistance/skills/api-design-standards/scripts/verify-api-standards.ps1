# scripts/verify-api-standards.ps1
Write-Host "Verifying API Design Standards..." -ForegroundColor Cyan

$hasErrors = $false

# Find the controllers directory
$controllersFolders = Get-ChildItem -Path "src" -Directory -Recurse -Filter "Controllers" | Where-Object { $_.FullName -match "API" }

if ($controllersFolders.Count -eq 0) {
    Write-Host "Warning: Controllers folder not found. Run this from the repository root." -ForegroundColor Yellow
    exit 0
}

foreach ($folder in $controllersFolders) {
    $controllers = Get-ChildItem -Path $folder.FullName -Filter "*Controller.cs" -Recurse

    foreach ($controller in $controllers) {
        $content = Get-Content $controller.FullName
        $text = $content -join "`n"

        # Check for try/catch blocks
        if ($text -match "catch\s*\(") {
            Write-Error "API Violation: '$($controller.Name)' contains a catch block. Exceptions must be handled by the Global Exception Middleware."
            $hasErrors = $true
        }

        # Check for hardcoded Route strings: e.g., [Route("api/users")]
        if ($text -match '\[Route\(\s*"[^"]+"\s*\)\]') {
            Write-Error "API Violation: '$($controller.Name)' contains a hardcoded route string. You must use APIUrlConstants."
            $hasErrors = $true
        }
        
        # Check for string interpolation logging
        if ($text -match '_logger\.Log.*\(.*\$".*"\)') {
             Write-Error "API Violation: '$($controller.Name)' contains string interpolation in logging. You must use structured logging."
             $hasErrors = $true
        }
    }
}

if ($hasErrors) {
    Write-Host "❌ Gate 1 Failed: API Design rules were violated." -ForegroundColor Red
    exit 1
}

Write-Host "✅ Gate 1 Passed: Controllers adhere to API standards." -ForegroundColor Green
exit 0
