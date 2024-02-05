using FluentAssertions;
using Xunit;

namespace ImportExtensions.UnitTests.Concrete
{
    public sealed class WhenStaticType
    {
        [Fact]
        public void ShouldBeExtension()
        {
            ImportExtensionsCommand.IsExtensionClass(typeof(ExampleClassExtensions)).Should().BeTrue();
        }

        [Fact]
        public void ShouldNotBeExtension()
        {
            ImportExtensionsCommand.IsExtensionClass(typeof(ExampleStatic)).Should().BeFalse();
        }
    }
}
