# scripts/verify-controller-authorization.ps1
Write-Host "Verifying Controller Authorization Attributes..." -ForegroundColor Cyan

$hasErrors = $false

# Locate Controllers
$controllersFolders = Get-ChildItem -Path "src" -Directory -Recurse -Filter "Controllers" | Where-Object { $_.FullName -match "API" }

if ($controllersFolders.Count -eq 0) {
    Write-Host "Warning: Controllers folder not found." -ForegroundColor Yellow
    exit 0
}

foreach ($folder in $controllersFolders) {
    $controllers = Get-ChildItem -Path $folder.FullName -Filter "*Controller.cs" -Recurse

    foreach ($controller in $controllers) {
        $content = Get-Content $controller.FullName
        $text = $content -join "`n"

        # Check if the class is globally secure or explicitly anonymous
        $classIsSecure = $text -match '\[Authorize' -or $text -match '\[AllowAnonymous\]' -or $text -match '\[TypeFilter\(typeof\(TokenAuthorizationFilterAttribute\)\)\]'

        if (-not $classIsSecure) {
            # Inspect individual methods if the class isn't globally secured.
            $lines = Get-Content $controller.FullName
            $methodAttributeBuffer = @()
            
            for ($i = 0; $i -lt $lines.Count; $i++) {
                $line = $lines[$i].Trim()
                
                # Buffer attributes
                if ($line.StartsWith("[")) {
                    $methodAttributeBuffer += $line
                }
                
                # Check when we hit a public endpoint method declaration
                if ($line -match 'public\s+(async\s+Task<IActionResult>|IActionResult|Task<ActionResult.*>|ActionResult)\s+\w+') {
                    $isMethodSecure = $false
                    foreach ($attr in $methodAttributeBuffer) {
                        if ($attr -match 'Authorize' -or $attr -match 'AllowAnonymous' -or $attr -match 'TokenAuthorizationFilterAttribute') {
                            $isMethodSecure = $true
                        }
                    }
                    
                    if (-not $isMethodSecure) {
                        Write-Error "Security Violation: Endpoint in '$($controller.Name)' starting with '$line' lacks authorization tags. Apply [Authorize], [AllowAnonymous], or custom filters."
                        $hasErrors = $true
                    }
                    
                    $methodAttributeBuffer = @()
                }
                
                # Reset buffer on non-attributes/comments/empty lines
                if (-not $line.StartsWith("[") -and -not $line.StartsWith("/") -and -not [string]::IsNullOrWhiteSpace($line) -and $line -notmatch 'public\s+(async\s+Task<IActionResult>|IActionResult|Task<ActionResult.*>|ActionResult)\s+\w+') {
                    $methodAttributeBuffer = @()
                }
            }
        }
    }
}

if ($hasErrors) {
    Write-Host "❌ Gate 1 (Auth check) Failed: Unsecured endpoints detected." -ForegroundColor Red
    exit 1
}

Write-Host "✅ Gate 1 Passed: All controllers and actions are explicitly secured or marked as AllowAnonymous." -ForegroundColor Green
exit 0
