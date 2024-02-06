using FluentAssertions;
using System.Reflection;
using Xunit;

namespace ImportExtensions.UnitTests.Concrete
{
#if !SKIP_TESTS
    public sealed class WhenReferenceMethod
    {
        [Fact]
        public void ShouldNotBeExtension()
        {
            MethodInfo method = typeof(ExampleClass).GetMethod(nameof(ExampleClass.Method));
            ImportExtensionsCommand
                .IsExtensionMethod(method)
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
