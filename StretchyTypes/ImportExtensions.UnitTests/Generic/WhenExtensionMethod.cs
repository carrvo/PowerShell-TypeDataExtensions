using FluentAssertions;
using System.Reflection;
using Xunit;

namespace ImportExtensions.UnitTests.Generic
{
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

        [Fact]
        public void Generic_ShouldBeExtension()
        {
            MethodInfo method = typeof(ExampleClassExtensions).GetMethod(nameof(ExampleClassExtensions.GenericMethod));
            ImportExtensionsCommand
                .IsExtensionMethod(method)
                .Should()
#if NET7_0_OR_GREATER
                .BeTrue();
#else
                .Be(true);
#endif
        }

        [Fact]
        public void Unbound_ShouldBeExtension()
        {
            MethodInfo method = typeof(ExampleClassExtensions).GetMethod(nameof(ExampleClassExtensions.UnboundMethod));
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
}
