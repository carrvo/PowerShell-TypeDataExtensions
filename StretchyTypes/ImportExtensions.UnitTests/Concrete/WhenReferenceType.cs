namespace ImportExtensions.UnitTests.Concrete
{
    public sealed class WhenReferenceType
    {
        [Fact]
        public void ShouldNotBeStatic()
        {
            ImportExtensionsCommand.IsStaticClass(typeof(ExampleClass)).Should().BeFalse();
        }
    }
}
