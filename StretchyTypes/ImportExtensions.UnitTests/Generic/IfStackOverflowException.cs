using FluentAssertions;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace ImportExtensions.UnitTests.Generic
{
    internal abstract class RecursiveType<TRecure>
    {
    }

    internal sealed class RecursedType : RecursiveType<RecursedType>
    {
    }

    internal static class RecursiveTypeExtensions
    {
        public static String RecursiveExtension<TRecure>(this IExampleClass example, TRecure recure)
            where TRecure : RecursiveType<TRecure>
        {
            return $"Hello from {nameof(RecursiveExtension)} with {typeof(TRecure).Name}";
        }

        public static String NonRecursiveExtension<TRecure1, TRecure2>(this IExampleClass example, TRecure1 recure)
            where TRecure1 : RecursiveType<RecursiveType<TRecure2>>
            where TRecure2 : RecursiveType<RecursiveType<String>>
        {
            return $"Hello from {nameof(NonRecursiveExtension)} with {typeof(TRecure1).Name}";
        }
        //public static String NonRecursiveExtension<TRecure>(this IExampleClass example, TRecure recure)
        //    where TRecure : RecursiveType<RecursiveType<String>>
        //{
        //    return $"Hello from {nameof(RecursiveExtension)} with {typeof(TRecure).Name}";
        //}
    }

    public sealed class IfStackOverflowException
    {
        [Fact]
        public void ShouldNotOverflow()
        {
            MethodInfo method = typeof(RecursiveTypeExtensions).GetMethod(nameof(RecursiveTypeExtensions.RecursiveExtension));
            ImportExtensionsCommand
                .IsExtensionMethod(method)
                .Should()
#if NET7_0_OR_GREATER
                .BeTrue();
#else
                .Be(true);
#endif

            var overflowParameter = method
                .GetParameters()
                .Skip(1)
                .First()
                .ParameterType;

            Action violator = () => overflowParameter.ToRecursivePSType();
            violator
                .Should()
                .NotThrow<StackOverflowException>();

            overflowParameter
                .ToRecursivePSType()
                .Should()
                .Match("ImportExtensions.UnitTests.Generic.RecursiveType[ImportExtensions.UnitTests.Generic.RecursiveType[*");
        }

        [Fact]
        public void NonRecursive_ShouldNotOverflow()
        {
            MethodInfo method = typeof(RecursiveTypeExtensions).GetMethod(nameof(RecursiveTypeExtensions.NonRecursiveExtension));
            ImportExtensionsCommand
                .IsExtensionMethod(method)
                .Should()
#if NET7_0_OR_GREATER
                .BeTrue();
#else
                .Be(true);
#endif

            var overflowParameter = method
                .GetParameters()
                .Skip(1)
                .First()
                .ParameterType;

            Action violator = () => overflowParameter.ToRecursivePSType();
            violator
                .Should()
                .NotThrow<StackOverflowException>();

            overflowParameter
                .ToRecursivePSType()
                .Should()
                .Be("ImportExtensions.UnitTests.Generic.RecursiveType[ImportExtensions.UnitTests.Generic.RecursiveType[ImportExtensions.UnitTests.Generic.RecursiveType[ImportExtensions.UnitTests.Generic.RecursiveType[System.String]]]]");
        }
    }
}
