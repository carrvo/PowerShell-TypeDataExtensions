using module .\bin\Debug\net7.0\StretchyTypes.psd1

Describe "ImportExtensions" {
	Context "When Importing" {
		Import-Extensions -Path '.\bin\Debug\net7.0\ImportExtensions.UnitTests.dll'

		It "Updates TypeData for Concrete Extension" {
			Get-TypeData -TypeName ImportExtensions.UnitTests.ExampleClass |
				Select-Object -ExpandProperty Members |
				Select-Object -ExpandProperty Keys |
				Should -Be "ExtensionMethod"
		}

		It "Maps Concrete Extension" {
			$example = New-Object ImportExtensions.UnitTests.ExampleClass
			$example.ExtensionMethod("me") | Should -Be "Hello me from ExtensionMethod"
		}

		It "Updates TypeData for Interface Extension" {
			Get-TypeData -TypeName ImportExtensions.UnitTests.IExampleClass |
				Select-Object -ExpandProperty Members |
				Select-Object -ExpandProperty Keys |
				Should -Be "ExtensionIMethod"
		}

		It "Maps Interface Extension" {
			$example = New-Object ImportExtensions.UnitTests.ExampleClass
			$example.ExtensionIMethod("me") | Should -Be "Hello me from ExtensionIMethod"
		}

		Register-Extensions -Generic ImportExtensions.UnitTests.Generic.ExampleClass -Specific int

		It "Updates TypeData for Generic Extension" {
			Get-TypeData |
				Where-Object TypeName -Match ImportExtensions.UnitTests.Generic.ExampleClass |
				Select-Object -ExpandProperty Members |
				Select-Object -First 1 -ExpandProperty Keys |
				Should -Be "ExtensionMethod"
		}

		It "Maps Generic Extension" {
			$example = New-Object ImportExtensions.UnitTests.Generic.ExampleClass[int]
			$example.ExtensionMethod("me") | Should -Be "Hello me from ExtensionMethod with Int32"
		}
	}
}