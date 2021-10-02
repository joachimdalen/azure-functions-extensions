param ([Switch] $IncreaseMajor, [Switch] $IncreaseMinor, [Switch] $IncreasePatch)

if (!(Get-Module -ListAvailable -Name SomeModule)) {
    Install-Module -Name SemVerPS -Force
} 


$endpointUri = "https://azuresearch-usnc.nuget.org/query?q=JoachimDalen.AzureFunctions.TestUtils&prerelease=true";
$body = (Invoke-WebRequest -Uri $endpointUri).Content

$data = ConvertFrom-Json -InputObject $body;
$version = $data[0].data.versions[-1].version;
$semVer = ConvertTo-SemVer -Version $version;

$major = $semVer.Major;
if($IncreaseMajor){
    $major = $major + 1;
}

$minor = $semVer.Minor;
if($IncreaseMinor){
    $minor = $minor + 1;
}

$patch = $semVer.Patch;
if($IncreasePatch){
    $patch = $patch + 1;
}


$newVersion = [Semver.SemVersion]::New($sem.Major, $sem.Minor, $sem.Patch + 1, $sem.Prerelease)
Write-Output "##vso[task.setvariable variable=UPDATED_PACKAGE_VERSION;isOutput=true]$newVersion"