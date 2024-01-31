namespace ImportExtensions.UnitTests.Concrete
{
    public sealed class WhenStaticMethod
    {
        [Fact]
        public void FromReferenceType_ShouldNotBeExtension()
        {
            ImportExtensionsCommand.IsExtensionMethod(typeof(ExampleClass).GetMethod(nameof(ExampleClass.StaticMethod))).Should().BeFalse();
        }

        [Fact]
        public void FromStaticType_ShouldNotBeExtension()
        {
            ImportExtensionsCommand.IsExtensionMethod(typeof(ExampleClassExtensions).GetMethod(nameof(ExampleClassExtensions.StaticMethod))).Should().BeFalse();
        }
    }
}
