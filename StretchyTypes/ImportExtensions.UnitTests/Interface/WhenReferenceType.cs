namespace ImportExtensions.UnitTests.Interface
{
    public sealed class WhenReferenceType
    {
        [Fact]
        public void ShouldNotBeStatic()
        {
            ImportExtensionsCommand.IsStaticClass(typeof(IExampleClass)).Should().BeFalse();
        }
    }
}
