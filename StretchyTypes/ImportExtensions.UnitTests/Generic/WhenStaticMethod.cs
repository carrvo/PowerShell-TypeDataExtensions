namespace ImportExtensions.UnitTests.Generic
{
    public sealed class WhenStaticMethod
    {
        [Fact]
        public void FromReferenceType_ShouldNotBeExtension()
        {
            ImportExtensionsCommand.IsExtensionMethod(typeof(ExampleClass<int>).GetMethod(nameof(ExampleClass<int>.StaticMethod))).Should().BeFalse();
        }

        [Fact]
        public void FromStaticType_ShouldNotBeExtension()
        {
            ImportExtensionsCommand.IsExtensionMethod(typeof(ExampleClassExtensions).GetMethod(nameof(ExampleClassExtensions.StaticMethod))).Should().BeFalse();
        }

        [Fact]
        public void FromStaticGeneric_ShouldNotBeExtension()
        {
            ImportExtensionsCommand.IsExtensionMethod(typeof(ExampleStatic<int>).GetMethod(nameof(ExampleStatic<int>.StaticMethod))).Should().BeFalse();
        }
    }
}
