using module ..\bin\Debug\StretchyTypes.psd1

Describe "ImportExtensions" {
	Context "When Importing" {
		switch ($PSVersionTable.PSVersion.Major) {
			7 {Import-Extensions -Path '..\bin\Debug\net7.0\ImportExtensions.UnitTests.dll'}
			5 {Import-Extensions -Path '..\bin\Debug\net48\ImportExtensions.UnitTests.dll'}
			default {Import-Extensions -Path '..\bin\Debug\netstandard2.0\ImportExtensions.UnitTests.dll'}
		}

		It "Updates TypeData for Concrete Extension" {
			Get-TypeData -TypeName ImportExtensions.UnitTests.ExampleClass |
				Select-Object -ExpandProperty Members |
				Select-Object -ExpandProperty Keys |
				Select-Object -First 1 |
				Should -Be "ExtensionMethod"
		}

		It "Maps Concrete Extension" {
			$example = New-Object ImportExtensions.UnitTests.ExampleClass
			$example.ExtensionMethod("me") | Should -Be "Hello me from ExtensionMethod"
		}

		Register-InterfaceExtensions -Interface ImportExtensions.UnitTests.IExampleClass -Concrete ImportExtensions.UnitTests.ExampleClass

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