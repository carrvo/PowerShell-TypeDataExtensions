<#
.SYNOPSIS
Exports TypeData to a *.ps1xml to be re-used with Update-TypeData.
.EXAMPLE
Get-TypeData | Export-TypeData -Path .\all.ps1xml
Exports all current TypeData to a single file.
#>
function Export-TypeData {

[CmdletBinding()]
Param(
    [Parameter(Mandatory, ValueFromPipeline)]
    [System.Management.Automation.Runspaces.TypeData]$TypeData,
    [Parameter(Mandatory)]
    [String]$Path,
    [Parameter()]
    [String[]]$Exclude
)

Begin {
Set-Content -Path $Path -Value "<?xml version=`"1.0`" encoding=`"utf-8`" ?>
<Types>"
}

Process {
Add-Content -Path $Path -Value "  <Type>
    <Name>$($TypeData.TypeName)</Name>
    <Members>
$($($TypeData.Members.Keys |
      Where-Object {$_ -NotIn $Exclude} |
      ForEach-Object {
        Switch($TypeData.Members.$_) {
            {$_ -Is [System.Management.Automation.Runspaces.AliasPropertyData]} {
"     <AliasProperty>
        <Name>$($_.Name)</Name>
        <ReferencedMemberName>$($_.ReferencedMemberName)</ReferencedMemberName>
      </AliasProperty>"
            }
            {$_ -Is [System.Management.Automation.Runspaces.CodeMethodData]} {
"      <CodeMethod>
        <Name>$($_.Name)</Name>
        <CodeReference>
          <TypeName>$($_.CodeReference.DeclaringType)</TypeName>
          <MethodName>$($_.CodeReference.Name)</MethodName>
        </CodeReference>
      </CodeMethod>"
            }
            {$_ -Is [System.Management.Automation.Runspaces.CodePropertyData]} {
"      <CodeProperty>
        <Name>$($_.Name)</Name>
        <GetCodeReference>
          <TypeName>
            $($_.GetCodeReference.DeclaringType)
          </TypeName>
          <MethodName>$($_.GetCodeReference.Name)</MethodName>
        </GetCodeReference>
      </CodeProperty>"
            }
            {$_ -Is [System.Management.Automation.Runspaces.NotePropertyData]} {
"      <NoteProperty>
        <Name>$($_.Name)</Name>
        <Value>$($_.Value)</Value>
      </NoteProperty>"
            }
            {$_ -Is [System.Management.Automation.Runspaces.ScriptMethodData]} {
"      <ScriptMethod>
        <Name>$($_.Name)</Name>
        <Script>
          $($_.Script)
        </Script>
      </ScriptMethod>"
            }
            {$_ -is [System.Management.Automation.Runspaces.ScriptPropertyData]} {
"      <ScriptProperty>
        <Name>$($_.Name)</Name>
        <GetScriptBlock>
        $($_.GetScriptBlock)
        </GetScriptBlock>
      </ScriptProperty>"
            }
        }
      }) -join "`n")
    </Members>
  </Type>"
}

End {
Add-Content -Path $Path -Value "</Types>
"
}

}

Export-ModuleMember -Function Export-TypeData
