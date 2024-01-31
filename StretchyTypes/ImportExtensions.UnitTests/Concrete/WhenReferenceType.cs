namespace ImportExtensions.UnitTests.Concrete
{
    public sealed class WhenReferenceType
    {
        [Fact]
        public void ShouldNotBeStatic()
        {
            ImportExtensionsCommand.IsExtensionClass(typeof(ExampleClass)).Should().BeFalse();
        }
    }
}
