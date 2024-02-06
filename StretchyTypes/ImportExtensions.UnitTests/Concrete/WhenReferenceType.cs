using FluentAssertions;
using Xunit;

namespace ImportExtensions.UnitTests.Concrete
{
#if !SKIP_TESTS
    public sealed class WhenReferenceType
    {
        [Fact]
        public void ShouldNotBeStatic()
        {
            ImportExtensionsCommand
                .IsExtensionClass(typeof(ExampleClass))
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
