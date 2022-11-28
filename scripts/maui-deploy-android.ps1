$signingKey = Read-Host -Prompt 'Input your keystore password';
$mauiProject = "../src/HeldInvoiceReleaser.Maui/HeldInvoiceReleaser.Maui.csproj";

$originalProject = Get-Content $mauiProject;

$originalProject
    | ForEach-Object{$_ -replace "<AndroidSigningKeyPass></AndroidSigningKeyPass>", "<AndroidSigningKeyPass>$signingKey</AndroidSigningKeyPass>"}
    | ForEach-Object {$_ -replace "<AndroidSigningStorePass></AndroidSigningStorePass>", "<AndroidSigningStorePass>$signingKey</AndroidSigningStorePass>"}
    | Set-Content $mauiProject;

& dotnet publish $mauiProject -f:net6.0-android -c:Release 

$originalProject | Set-Content $mauiProject
