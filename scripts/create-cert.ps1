$certStoreLocation = 'cert:\localMachine\my\'
$dnsName = Read-Host -Prompt 'Input your desired dns name'
$certPassword = Read-Host -Prompt 'Create password for certificate'
$outputDirectory = "$env:TEMP\certificate"
$outputPath = "$outputDirectory\$dnsName.pfx"

$cert = New-SelfSignedCertificate -certstorelocation cert:\localmachine\my -dnsname $dnsName
$password = ConvertTo-SecureString -String $certPassword -Force -AsPlainText
$path = $certStoreLocation + $cert.thumbprint
Write-Host "Creating certificate at $outputPath"
New-Item $outputDirectory -ItemType Directory -ea 0
Export-PfxCertificate -cert $path -FilePath $outputPath -Password $password

Invoke-Item $outputDirectory