using module .\bin\Debug\net7.0\ImportExtensions.dll
#using assembly .\bin\Debug\net7.0\ImportExtensions.UnitTests.dll

Describe "ImportExtensions" {
	Context "When Importing" {
		$assembly = [System.Reflection.Assembly]::LoadFile($(Get-Item '.\bin\Debug\net7.0\ImportExtensions.UnitTests.dll' | Select-Object -ExpandProperty FullName))
		Import-Extensions -Assembly $assembly

		It "Maps Extension" {
			<#Get-TypeData -TypeName ImportExtensions.UnitTests.ExampleClass |
				Select-Object -ExpandProperty Members#>
			$example = New-Object ImportExtensions.UnitTests.ExampleClass
			$example.ExtensionMethod("me") | Should Be "Hello me from ExtensionMethod"
		}
	}
}