using ImportExtensions.UnitTests;

namespace ImportExtensions.UnitTests.Interface
{
    public sealed class WhenExtensionMethod
    {
        [Fact]
        public void ShouldBeExtension()
        {
            ImportExtensionsCommand.IsExtensionMethod(typeof(ExampleClassExtensions).GetMethod(nameof(ExampleClassExtensions.ExtensionIMethod))).Should().BeTrue();
        }
    }
}
