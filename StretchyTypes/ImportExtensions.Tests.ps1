using module .\bin\Debug\net7.0\ImportExtensions.dll

Describe "ImportExtensions" {
	Context "When Importing" {
		Import-Extensions -Path '.\bin\Debug\net7.0\ImportExtensions.UnitTests.dll'

		It "Maps Concrete Extension" {
			<#Get-TypeData -TypeName ImportExtensions.UnitTests.ExampleClass |
				Select-Object -ExpandProperty Members#>
			$example = New-Object ImportExtensions.UnitTests.ExampleClass
			$example.ExtensionMethod("me") | Should -Be "Hello me from ExtensionMethod"
		}

		It "Maps Interface Extension" {
			$example = New-Object ImportExtensions.UnitTests.ExampleClass
			$example.ExtensionIMethod("me") | Should -Be "Hello me from ExtensionIMethod"
		}

		It "Maps Generic Extension" {
			$example = New-Object ImportExtensions.UnitTests.Generic.ExampleClass[int]
			$example.ExtensionMethod("me") | Should -Be "Hello me from ExtensionMethod with Int32"
		}
	}
}