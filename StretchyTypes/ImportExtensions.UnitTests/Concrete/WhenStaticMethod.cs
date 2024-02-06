using FluentAssertions;
using System.Reflection;
using Xunit;

namespace ImportExtensions.UnitTests.Concrete
{
    public sealed class WhenStaticMethod
    {
        [Fact]
        public void FromReferenceType_ShouldNotBeExtension()
        {
            MethodInfo method = typeof(ExampleClass).GetMethod(nameof(ExampleClass.StaticMethod));
            ImportExtensionsCommand
                .IsExtensionMethod(method)
                .Should()
#if NET7_0_OR_GREATER
                .BeFalse();
#else
                .Be(false);
#endif
        }

        [Fact]
        public void FromStaticType_ShouldNotBeExtension()
        {
            MethodInfo method = typeof(ExampleClassExtensions).GetMethod(nameof(ExampleClassExtensions.StaticMethod));
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
}
