using FluentAssertions;
using Xunit;

namespace ImportExtensions.UnitTests.Interface
{
    public sealed class WhenStaticMethod
    {
        [Fact]
        public void FromStaticType_ShouldNotBeExtension()
        {
            ImportExtensionsCommand.IsExtensionMethod(typeof(ExampleClassExtensions).GetMethod(nameof(ExampleClassExtensions.StaticIMethod))).Should().BeFalse();
        }
    }
}
