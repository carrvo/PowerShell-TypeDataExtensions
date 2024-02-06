using FluentAssertions;
using Xunit;

namespace ImportExtensions.UnitTests.Concrete
{
#if !SKIP_TESTS
    public sealed class WhenStaticType
    {
        [Fact]
        public void ShouldBeExtension()
        {
            ImportExtensionsCommand
                .IsExtensionClass(typeof(ExampleClassExtensions))
                .Should()
#if NET7_0_OR_GREATER
                .BeTrue();
#else
                .Be(true);
#endif
        }

        [Fact]
        public void ShouldNotBeExtension()
        {
            ImportExtensionsCommand
                .IsExtensionClass(typeof(ExampleStatic))
                .Should()
#if NET7_0_OR_GREATER
                .BeFalse();
#else
                .Be(false);
#endif
        }
    }
#endif
}
