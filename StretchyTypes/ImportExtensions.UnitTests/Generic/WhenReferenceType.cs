namespace ImportExtensions.UnitTests.Generic
{
    public sealed class WhenReferenceType
    {
        [Fact]
        public void ShouldNotBeStatic()
        {
            ImportExtensionsCommand.IsExtensionClass(typeof(ExampleClass<int>)).Should().BeFalse();
        }
    }
}
