param ([Switch] $IncreaseMajor, [Switch] $IncreaseMinor, [Switch] $IncreasePatch, [string] $SelectedVersion)

if ($SelectedVersion -ne "not-set") {
    Write-Host "Version set manually";
    Write-Output "##vso[task.setvariable variable=UPDATED_PACKAGE_VERSION;isOutput=true]$SelectedVersion"
}
else {
    if (!(Get-Module -ListAvailable -Name SemVerPS)) {
        Write-Host "Installing SemVerPS";
        Install-Module -Name SemVerPS -Force
    } 
    
    
    $endpointUri = "https://azuresearch-usnc.nuget.org/query?q=JoachimDalen.AzureFunctions.TestUtils&prerelease=true";
    $body = (Invoke-WebRequest -Uri $endpointUri).Content
    
    $data = ConvertFrom-Json -InputObject $body;
    $version = $data[0].data.versions[-1].version;
    $semVer = ConvertTo-SemVer -Version $version;
    
    Write-Host "Found existing package version $semVer";
    
    $major = $semVer.Major;
    if ($IncreaseMajor) {
        Write-Host "Updating major version"
        $major = $major + 1;
    }
    
    $minor = $semVer.Minor;
    if ($IncreaseMinor) {
        Write-Host "Updating minor version"
        $minor = $minor + 1;
    }
    
    $patch = $semVer.Patch;
    if ($IncreasePatch) {
        Write-Host "Updating patch version"
        $patch = $patch + 1;
    }
    
    $newVersion = [Semver.SemVersion]::New($sem.Major, $sem.Minor, $sem.Patch + 1, $sem.Prerelease)
    
    Write-Host "Setting new version $newVersion"
    Write-Output "##vso[task.setvariable variable=UPDATED_PACKAGE_VERSION;isOutput=true]$newVersion"
}

