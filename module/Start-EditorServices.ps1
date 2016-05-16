param(
    [string]
    $EditorServicesVersion,

    # Mutually exclusive!

    [string]
    $EditorServicesModulePath,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]
    $HostName,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]
    $HostProfileName,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]
    $HostVersion,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]
    $NamedPipeServerName,

    [switch]
    $WaitForCompletion
)

if ($EditorServicesModulePath -ne $null) {
    Import-Module "$EditorServicesModulePath\PowerShellEditorServices.psd1" -ErrorAction Stop
}
else {
    Import-Module PowerShellEditorServices -RequiredVersion [System.Version]::new($EditorServicesVersion) -ErrorAction Stop
}


Start-LanguageServer -HostName $HostName -HostProfileName $HostProfileName

# Run this script with the following command:
# powershell.exe -NoProfile -NonInteractive -ExecutionPolicy Unrestricted -Command ".\Start-EditorServices.ps1"

# powershell.exe -NoProfile -NonInteractive -ExecutionPolicy Unrestricted -Command ".\Start-EditorServices.ps1 -EditorServicesModulePath \"c:\dev\PowerShellEditorServices\module\PowerShellEditorServices\\""