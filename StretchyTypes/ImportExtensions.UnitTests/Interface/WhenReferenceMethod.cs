﻿using FluentAssertions;
using System.Reflection;
using Xunit;

namespace ImportExtensions.UnitTests.Interface
{
    public sealed class WhenReferenceMethod
    {
        [Fact]
        public void ShouldNotBeExtension()
        {
            MethodInfo method = typeof(ExampleClass).GetMethod(nameof(IExampleClass.Method));
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
