param(
    [string]$Configuration = "Release",
    [string]$Platform = "Any CPU"
)

# Stop on any error
$ErrorActionPreference = "Stop"

Write-Host "🚀 Starting build process..." -ForegroundColor Cyan

# Ensure we're in the right directory
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $scriptPath

# Clean previous builds
Write-Host "🧹 Cleaning previous builds..." -ForegroundColor Yellow
dotnet clean PhoneMouse/PhoneMouse.sln

# Restore NuGet packages
Write-Host "📦 Restoring NuGet packages..." -ForegroundColor Yellow
dotnet restore PhoneMouse/PhoneMouse.sln

# Build the solution
Write-Host "🔨 Building solution..." -ForegroundColor Yellow
dotnet build PhoneMouse/PhoneMouse.sln --configuration $Configuration --no-restore

# Publish the application
Write-Host "📤 Publishing application..." -ForegroundColor Yellow
dotnet publish PhoneMouse/PhoneMouse.sln --configuration $Configuration --no-build

Write-Host "✅ Build completed successfully!" -ForegroundColor Green 