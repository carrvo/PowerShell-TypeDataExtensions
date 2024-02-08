using FluentAssertions;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;

namespace ImportExtensions.UnitTests.Concrete
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

            method
                .GetParameters()
                .First()
                .ParameterType
                .ToPSType()
                .Should()
                .Be($"{typeof(ExampleClass).Namespace}.ExampleClass");
        }

        [Fact]
        public void Property_ShouldBeExtension()
        {
            MethodInfo method = typeof(ExampleClassExtensions).GetMethod(nameof(ExampleClassExtensions.ExtensionProperty));
            ImportExtensionsCommand
                .IsExtensionMethod(method)
                .Should()
#if NET7_0_OR_GREATER
                .BeTrue();
#else
                .Be(true);
#endif

            method
                .GetParameters()
                .First()
                .ParameterType
                .ToPSType()
                .Should()
                .Be($"{typeof(ExampleClass).Namespace}.ExampleClass");

            method
                .GetParameters()
                .Should()
                .HaveCount(1);
        }

        [Fact]
        public void Reference_ShouldBeExtension()
        {
            MethodInfo method = typeof(ExampleClassExtensions).GetMethod(nameof(ExampleClassExtensions.ExtensionReference));
            ImportExtensionsCommand
                .IsExtensionMethod(method)
                .Should()
#if NET7_0_OR_GREATER
                .BeTrue();
#else
                .Be(true);
#endif

            method
                .GetParameters()
                .First()
                .ParameterType
                .ToPSType()
                .Should()
                .Be($"{typeof(ExampleClass).Namespace}.ExampleClass");

            method
                .GetParameters()
                .Skip(1)
                .First()
                .ParameterType
                .ToRecursivePSType()
                .Should()
                .Be("ref");
        }
    }
}
