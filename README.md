# PowerShell-TypeDataExtensions

This project includes various utilities to augment PowerShell's [TypeData](https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.core/about/about_types.ps1xml) feature.

## Export

This allows you to export in-flight TypeData modifications (through `Update-TypeData`) to a file that can be re-loaded.

See the following for documentation:
```PowerShell
Import-Module ...\Export\ExportTypeData.psd1
Get-Help Export-TypeData
```

## StretchyTypes

This aims to bring C# extension methods so that they can be used natively in PowerShell!
This way you can enjoy the same syntactic sugar for the same methods regardless of your language.

It will search a given assembly and convert all found extension methods to TypeData.

Please see [the tests](./StretchyTypes/ImportExtensions.Examples/ImportExtensions.Tests.ps1) for example usage.
