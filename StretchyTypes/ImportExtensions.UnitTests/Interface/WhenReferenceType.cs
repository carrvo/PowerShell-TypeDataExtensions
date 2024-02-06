using FluentAssertions;
using Xunit;

namespace ImportExtensions.UnitTests.Interface
{
    public sealed class WhenReferenceType
    {
        [Fact]
        public void ShouldNotBeStatic()
        {
            ImportExtensionsCommand
                .IsExtensionClass(typeof(IExampleClass))
                .Should()
#if NET7_0_OR_GREATER
                .BeFalse();
#else
                .Be(false);
#endif
        }
    }
}
