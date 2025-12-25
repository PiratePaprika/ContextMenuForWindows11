if ($env:VERSION_BUILD_NUMBER) {
    $manifestPaths = @(
        ".\ContextMenuCustom\ContextMenuCustomDevPackage\Package.appxmanifest",
        ".\ContextMenuCustom\ContextMenuCustomGithubPackage\Package.appxmanifest",
        ".\ContextMenuCustom\ContextMenuCustomAnyPackage\Package.appxmanifest",
        ".\ContextMenuCustom\ContextMenuCustomPackage\Package.appxmanifest"
    )

    foreach ($manifestPath in $manifestPaths) {
        if (-not (Test-Path -LiteralPath $manifestPath)) {
            Write-Warning "Manifest not found: $manifestPath"
            continue
        }

        $appxManifestPath = Convert-Path $manifestPath
        [xml]$manifest = Get-Content -Path $appxManifestPath
        $version = $manifest.Package.Identity.Version
        $versionParts = $version -split '\.'

        if ($versionParts.Length -ne 4) {
            Write-Error "Version format error: $version ($appxManifestPath)"
            exit 1
        }

        $versionParts[3] = $env:VERSION_BUILD_NUMBER
        $manifest.Package.Identity.Version = $versionParts -join "."
        Write-Host "package new version ($appxManifestPath): $($manifest.Package.Identity.Version)"
        $manifest.Save($appxManifestPath)
    }
}