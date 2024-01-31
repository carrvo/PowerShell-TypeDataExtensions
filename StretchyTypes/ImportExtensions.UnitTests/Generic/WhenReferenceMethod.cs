namespace ImportExtensions.UnitTests.Generic
{
    public sealed class WhenReferenceMethod
    {
        [Fact]
        public void ShouldNotBeExtension()
        {
            ImportExtensionsCommand.IsExtensionMethod(typeof(ExampleClass<int>).GetMethod(nameof(ExampleClass<int>.Method))).Should().BeFalse();
        }
    }
}
