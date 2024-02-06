using FluentAssertions;
using System.Reflection;
using Xunit;

namespace ImportExtensions.UnitTests.Generic
{
#if !SKIP_TESTS
    public sealed class WhenExtensionMethod
    {
        [Fact]
        public void ShouldBeExtension()
        {
            MethodInfo method = typeof(ExampleClassExtensions).GetMethod(nameof(ExampleClassExtensions.ExtensionMethod));
            ImportExtensionsCommand
                .IsExtensionMethod(method)
                .Should()
#if NET7_0_OR_GREATER
                .BeTrue();
#else
                .Be(true);
#endif
        }
    }
#endif
}
