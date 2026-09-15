# scripts/verify-security-standards.ps1
Write-Host "Verifying Security Governance Standards..." -ForegroundColor Cyan

$hasErrors = $false

# 1. Check for Hardcoded Secrets in C# files
$csFiles = Get-ChildItem -Path "src" -Filter "*.cs" -Recurse -ErrorAction SilentlyContinue

if ($csFiles) {
    foreach ($file in $csFiles) {
        $content = Get-Content $file.FullName
        $text = $content -join "`n"

        # Extremely rudimentary check for hardcoded symmetric keys (e.g. GetBytes("SomeSuperSecretKey12345!"))
        if ($text -match 'GetBytes\(\s*"[A-Za-z0-9@#\$%\^&\*\(\)-_=\+]{10,}"\s*\)') {
            Write-Error "Security Violation: '$($file.Name)' appears to contain a hardcoded encryption/signing key in GetBytes(). Use IConfiguration instead."
            $hasErrors = $true
        }
        
        # Check for inline AES creation instead of Helper
        if ($file.Name -ne "Encryption.cs" -and $text -match 'Aes\.Create\(\)') {
             Write-Error "Security Violation: '$($file.Name)' creates AES manually. You MUST use the Encryption helper class."
             $hasErrors = $true
        }
    }
}

# 2. Check Pipeline Order in Program.cs
$programFiles = Get-ChildItem -Path "src" -Filter "Program.cs" -Recurse -ErrorAction SilentlyContinue
foreach ($file in $programFiles) {
    $content = Get-Content $file.FullName
    $text = $content -join "`n"

    if ($text -match 'UseAuthorization' -and $text -match 'UseAuthentication') {
        $authIdx = $text.IndexOf("UseAuthentication")
        $authzIdx = $text.IndexOf("UseAuthorization")

        if ($authzIdx -lt $authIdx) {
            Write-Error "Security Violation: '$($file.Name)' has UseAuthorization before UseAuthentication. Authentication MUST come first."
            $hasErrors = $true
        }
    }
}

if ($hasErrors) {
    Write-Host "❌ Gate 1 Failed: Security governance rules were violated." -ForegroundColor Red
    exit 1
}

Write-Host "✅ Gate 1 Passed: No security violations detected." -ForegroundColor Green
exit 0
