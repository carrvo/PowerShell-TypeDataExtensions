using FluentAssertions;
using Xunit;

namespace ImportExtensions.UnitTests.Generic
{
    public sealed class WhenStaticType
    {
        [Fact]
        public void ShouldBeExtension()
        {
            ImportExtensionsCommand
                .IsExtensionClass(typeof(ExampleStatic<int>))
                .Should()
#if NET7_0_OR_GREATER
                .BeFalse();
#else
                .Be(false);
#endif
        }
    }
}
