#Add-Type -Path "..\..\src\PowerShellEditorServices\bin\Debug\Microsoft.PowerShell.EditorServices.dll"
Add-Type -Path "$PSScriptRoot\..\..\src\PowerShellEditorServices.Protocol\bin\Debug\Microsoft.PowerShell.EditorServices.Protocol.dll"
function New-EditorSession() {
    return [Microsoft.PowerShell.EditorServices.EditorSession]::new()
}

function Start-LanguageServer {
    [CmdletBinding()]
    param(
        [string]
        $HostName,

        [string]
        $HostProfileName,

        [string]
        $HostVersion,

        [string]
        $NamedPipeServerName

        [switch]
        $WaitForCompletion
    )

    $hostDetails = [Microsoft.PowerShell.EditorServices.Session.HostDetails]::new($HostName, $HostProfileName, [System.Version]::new($HostVersion))
    $languageServer = [Microsoft.PowerShell.EditorServices.Protocol.Server.LanguageServer]::new($hostDetails)

    $languageServer.Start().Wait();

    Write-Output "INITIALIZED"

    if ($WaitForCompletion.IsPresent) {
        Write-Output "Waiting for exit!"
        $languageServer.WaitForExit();
    }

    return $languageServer
}

function Stop-LanguageServer($languageServer) {

}