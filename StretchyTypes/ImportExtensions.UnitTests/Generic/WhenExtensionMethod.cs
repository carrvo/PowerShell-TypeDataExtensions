namespace ImportExtensions.UnitTests.Generic
{
    public sealed class WhenExtensionMethod
    {
        [Fact]
        public void ShouldBeExtension()
        {
            ImportExtensionsCommand.IsExtensionMethod(typeof(ExampleClassExtensions).GetMethod(nameof(ExampleClassExtensions.ExtensionMethod))).Should().BeTrue();
        }
    }
}
