using FluentAssertions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;
using System.Reflection;
using Xunit;

namespace ImportExtensions.UnitTests.Generic
{
    public sealed class WhenConvertingToScriptBlock
    {
        public ImportExtensionsCommand Sut { get; }
        public ExampleClass<int> Input { get; }

        public WhenConvertingToScriptBlock()
        {
            RunspaceWrapper.SetDefaultRunspace();
            RunspaceWrapper.RunspaceExecution.Should().NotBeNull();

            Sut = new ImportExtensionsCommand();
            Input = new ExampleClass<int>();
        }

        [Fact]
        public void ShouldBeCallable()
        {
            var extensionMethod = typeof(ExampleClassExtensions).GetMethod(nameof(ExampleClassExtensions.ExtensionMethod));
            extensionMethod.Should().NotBeNull();

            var scriptBlock = ScriptBlock.Create(Sut.ToScriptBlock(extensionMethod));
            var output = scriptBlock.Invoke(nameof(ShouldBeCallable)).Single().BaseObject;

            output.Should().BeOfType<string>();
            var str = output as string;
            str.Should().Be($"Hello {nameof(ShouldBeCallable)} from {nameof(ExampleClassExtensions.ExtensionMethod)} with {typeof(int).Name}");
        }

        [Fact]
        public void Complex_ShouldBeCallable()
        {
            var extensionMethod = typeof(ExampleClassExtensions).GetMethod(nameof(ExampleClassExtensions.Complex));
            extensionMethod.Should().NotBeNull();

            var scriptBlock = ScriptBlock.Create(Sut.ToScriptBlock(extensionMethod));
            Func<Object, Object> expression = x => x;
            var output = scriptBlock.Invoke(expression).Single().BaseObject;

            output.Should().BeOfType<string>();
            var str = output as string;
            str.Should().Be($"Hello from {nameof(ExampleClassExtensions.Complex)}");
        }
    }
}
