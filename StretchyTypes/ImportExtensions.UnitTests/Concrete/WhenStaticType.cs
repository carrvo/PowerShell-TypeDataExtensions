namespace ImportExtensions.UnitTests.Concrete
{
    public sealed class WhenStaticType
    {
        [Fact]
        public void ShouldBeExtension()
        {
            ImportExtensionsCommand.IsStaticClass(typeof(ExampleClassExtensions)).Should().BeTrue();
        }

        [Fact]
        public void ShouldNotBeExtension()
        {
            ImportExtensionsCommand.IsStaticClass(typeof(ExampleStatic)).Should().BeFalse();
        }
    }
}
