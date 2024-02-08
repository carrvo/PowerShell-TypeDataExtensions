using module ..\bin\Debug\netstandard2.0\StretchyTypes.psd1

Describe "ImportExtensions" {
	Context "When Importing" {
		Import-Extensions -Path '.\bin\Debug\netstandard2.0\ImportExtensions.Tests.Common.dll'

		It "Updates TypeData for Concrete Extension" {
			Get-TypeData -TypeName ImportExtensions.UnitTests.ExampleClass |
				Select-Object -ExpandProperty Members |
				Select-Object -ExpandProperty Keys |
				Should -Contain "ExtensionMethod"
		}

		It "Maps Concrete Extension" {
			$example = New-Object ImportExtensions.UnitTests.ExampleClass
			$example.ExtensionMethod("me") | Should -Be "Hello me from ExtensionMethod"
		}

		It "Maps Concrete Property Extension" {
			$example = New-Object ImportExtensions.UnitTests.ExampleClass
			$example.ExtensionProperty() | Should -Be "Hello from ExtensionProperty"
		}

		It "Maps Concrete Reference Extension" {
			$example = New-Object ImportExtensions.UnitTests.ExampleClass
			$example.ExtensionReference(([ref]"me")) | Should -Be "Hello me from ExtensionReference"
		}

		Copy-Extensions -From ImportExtensions.UnitTests.IExampleClass -To ImportExtensions.UnitTests.ExampleClass

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

		Register-GenericExtensions -Generic ImportExtensions.UnitTests.Generic.ExampleClass -Specific int

		It "Updates TypeData for Generic Type Extension" {
			Get-TypeData |
				Where-Object TypeName -Match ImportExtensions.UnitTests.Generic.ExampleClass |
				Select-Object -ExpandProperty Members |
				Select-Object -ExpandProperty Keys |
				Should -Contain "ExtensionMethod"
		}

		It "Maps Generic Type Extension" {
			$example = New-Object ImportExtensions.UnitTests.Generic.ExampleClass[int]
			$example.ExtensionMethod("me") | Should -Be "Hello me from ExtensionMethod with Int32"
		}
		
		Copy-Extensions -From System.Object -To ImportExtensions.UnitTests.ExampleClass

		It "Updates TypeData for Generic Extension" {
			Get-TypeData |
				Where-Object TypeName -Match ImportExtensions.UnitTests.ExampleClass |
				Select-Object -ExpandProperty Members |
				Select-Object -ExpandProperty Keys |
				Should -Contain "GenericMethod"
		}

		It "Maps Generic Extension" {
			$example = New-Object ImportExtensions.UnitTests.ExampleClass
			$example.GenericMethod("me") | Should -Be "Hello me from GenericMethod with ExampleClass"
		}

		It "Maps Unbound Extension" {
			$example = New-Object ImportExtensions.UnitTests.ExampleClass
			$example.UnboundMethod("me") | Should -Be "Hello me from UnboundMethod with ExampleClass"
		}
	}
}