# scripts/verify-dependencies.ps1
Write-Host "Verifying Architectural Dependencies..." -ForegroundColor Cyan

$hasErrors = $false

# Define the allowed dependencies for each layer
$allowedDependencies = @{
    "Core.API" = @("Core.Service", "Core.DTO")
    "Core.Service" = @("Core.Repository", "Core.DTO", "Core.Entities")
    "Core.Repository" = @("Core.Entities")
    "Core.DTO" = @("Core.Entities")
    "Core.Entities" = @() # Must be pure
}

# Recursively find all csproj files in the src directory
if (-not (Test-Path "src")) {
    Write-Host "Warning: 'src' folder not found. Run this from the repository root." -ForegroundColor Yellow
    exit 0
}

$projectFiles = Get-ChildItem -Path "src" -Filter "*.csproj" -Recurse

foreach ($proj in $projectFiles) {
    [xml]$xml = Get-Content $proj.FullName
    $projName = $proj.BaseName
    
    # Identify which layer this project belongs to
    $layer = $null
    foreach ($key in $allowedDependencies.Keys) {
        if ($projName -match $key) {
            $layer = $key
            break
        }
    }

    if ($layer) {
        $references = $xml.SelectNodes("//ProjectReference")
        foreach ($ref in $references) {
            $refPath = $ref.Include
            $refName = [System.IO.Path]::GetFileNameWithoutExtension($refPath)
            
            # Check if the referenced project is one of our internal layers
            $refLayer = $null
            foreach ($key in $allowedDependencies.Keys) {
                if ($refName -match $key) {
                    $refLayer = $key
                    break
                }
            }

            if ($refLayer -and $allowedDependencies[$layer] -notcontains $refLayer) {
                Write-Error "Architecture Violation: Project '$projName' is NOT allowed to reference '$refName'."
                $hasErrors = $true
            }
        }
    }
}

if ($hasErrors) {
    Write-Host "❌ Gate 1 Failed: Architectural dependency rules were violated. Please fix the project references." -ForegroundColor Red
    exit 1
}

Write-Host "✅ Gate 1 Passed: No invalid project references found." -ForegroundColor Green
exit 0
