namespace ImportExtensions.UnitTests
{
    public sealed class WhenStaticType
    {
        [Fact]
        public void ShouldBeStatic()
        {
            ImportExtensionsCommand.IsStaticClass(typeof(ExampleClassExtensions)).Should().BeTrue();
        }
    }
}
