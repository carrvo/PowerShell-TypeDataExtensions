using module .\bin\Debug\net7.0\ImportExtensions.dll

Describe "ImportExtensions" {
	Context "When Importing" {
		Import-Extensions -Path '.\bin\Debug\net7.0\ImportExtensions.UnitTests.dll'

		It "Maps Extension" {
			<#Get-TypeData -TypeName ImportExtensions.UnitTests.ExampleClass |
				Select-Object -ExpandProperty Members#>
			$example = New-Object ImportExtensions.UnitTests.ExampleClass
			$example.ExtensionMethod("me") | Should -Be "Hello me from ExtensionMethod"
		}
	}
}