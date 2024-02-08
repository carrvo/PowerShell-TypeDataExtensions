using FluentAssertions;
using System.Linq.Expressions;
using System.Linq;
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

            method
                .GetParameters()
                .First()
                .ParameterType
                .ToPSType()
                .Should()
                .Be($"{typeof(ExampleClass<int>).Namespace}.ExampleClass[T]");
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

            method
                .GetParameters()
                .First()
                .ParameterType
                .ToPSType()
                .Should()
                .Be($"{typeof(UnitTests.ExampleClass).Namespace}.ExampleClass");
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

            method
                .GetParameters()
                .First()
                .ParameterType
                .ToPSType()
                .Should()
                .Be("System.Object");
        }

        [Fact]
        public void Complex_ShouldBeExtension()
        {
            MethodInfo method = typeof(ExampleClassExtensions).GetMethod(nameof(ExampleClassExtensions.Complex));
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
                .Skip(1)
                .First()
                .ParameterType
                .ToPSType()
                .Should()
                .Be($"{typeof(Expression).Namespace}.Expression[System.Func[T,System.Object]]");
        }
    }
}
