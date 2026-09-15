# scripts/verify-tests.ps1
Write-Host "Verifying Testing Standards..." -ForegroundColor Cyan

$hasErrors = $false

# Locate Test projects
$testProjects = Get-ChildItem -Path "tests" -Filter "*.cs" -Recurse -ErrorAction SilentlyContinue

if (-not $testProjects) {
    Write-Host "Warning: No test files found." -ForegroundColor Yellow
    exit 0
}

foreach ($test in $testProjects) {
    $content = Get-Content $test.FullName
    $text = $content -join "`n"

    # Check for AAA pattern
    if ($text -match '\[Test\]' -and -not ($text -match '// Arrange' -and $text -match '// Act' -and $text -match '// Assert')) {
        Write-Error "Test Violation: '$($test.Name)' is missing the standard '// Arrange', '// Act', '// Assert' structure."
        $hasErrors = $true
    }

    # Check for direct SQL Server usage (DbContext creation without in-memory or mocks)
    if ($text -match 'new .*Context\(' -and -not $text -match 'UseInMemoryDatabase') {
        Write-Error "Test Violation: '$($test.Name)' appears to instantiate a DbContext directly without using an InMemoryDatabase or mocking."
        $hasErrors = $true
    }
}

if ($hasErrors) {
    Write-Host "❌ Gate 1 Failed: Testing rules were violated." -ForegroundColor Red
    exit 1
}

Write-Host "✅ Gate 1 Passed: All tests follow the AAA pattern and mocking standards." -ForegroundColor Green
exit 0
