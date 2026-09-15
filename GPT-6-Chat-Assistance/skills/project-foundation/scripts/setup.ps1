# scripts/setup.ps1
Write-Host "Initializing <ProjectName> Repository..." -ForegroundColor Cyan

# 1. Ensure required directories exist (auto-fix for basic scaffolding)
$requiredDirectories = @("src", "tests", "docs", "scripts", "build")
foreach ($dir in $requiredDirectories) {
    if (-not (Test-Path -Path $dir -PathType Container)) {
        Write-Host "Creating missing directory: $dir"
        New-Item -ItemType Directory -Path $dir | Out-Null
    }
}

# 2. Restore .NET tools if a tool manifest exists
if (Test-Path ".config\dotnet-tools.json") {
    Write-Host "Restoring .NET local tools..."
    dotnet tool restore
}

Write-Host "Repository initialization complete!" -ForegroundColor Green
