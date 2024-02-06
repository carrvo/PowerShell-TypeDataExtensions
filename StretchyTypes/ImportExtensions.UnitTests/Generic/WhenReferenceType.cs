using FluentAssertions;
using Xunit;

namespace ImportExtensions.UnitTests.Generic
{
    public sealed class WhenReferenceType
    {
        [Fact]
        public void ShouldNotBeStatic()
        {
            ImportExtensionsCommand
                .IsExtensionClass(typeof(ExampleClass<int>))
                .Should()
#if NET7_0_OR_GREATER
                .BeFalse();
#else
                .Be(false);
#endif
        }
    }
}
