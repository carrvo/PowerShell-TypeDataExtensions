﻿namespace ImportExtensions.UnitTests
{
    public sealed class WhenReferenceMethod
    {
        [Fact]
        public void ShouldNotBeExtension()
        {
            ImportExtensionsCommand.IsExtensionMethod(typeof(ExampleClass).GetMethod(nameof(ExampleClass.Method))).Should().BeFalse();
        }
    }
}
