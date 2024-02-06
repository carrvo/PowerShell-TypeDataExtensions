using FluentAssertions;
using System.Reflection;
using Xunit;

namespace ImportExtensions.UnitTests.Generic
{
#if !SKIP_TESTS
    public sealed class WhenReferenceMethod
    {
        [Fact]
        public void ShouldNotBeExtension()
        {
            MethodInfo method = typeof(ExampleClass<int>).GetMethod(nameof(ExampleClass<int>.Method));
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
