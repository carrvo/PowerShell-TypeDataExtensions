using FluentAssertions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;
using System.Reflection;
using Xunit;

namespace ImportExtensions.UnitTests.Concrete
{
    public sealed class WhenConvertingToScriptBlock
    {
        public ImportExtensionsCommand Sut { get; }
        public ExampleClass Input { get; }

        public WhenConvertingToScriptBlock()
        {
            RunspaceWrapper.SetDefaultRunspace();
            RunspaceWrapper.RunspaceExecution.Should().NotBeNull();

            Sut = new ImportExtensionsCommand();
            Input = new ExampleClass();
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
            str.Should().Be($"Hello {nameof(ShouldBeCallable)} from {nameof(ExampleClassExtensions.ExtensionMethod)}");
        }

        [Fact]
        public void Property_ShouldBeCallable()
        {
            var extensionMethod = typeof(ExampleClassExtensions).GetMethod(nameof(ExampleClassExtensions.ExtensionProperty));
            extensionMethod.Should().NotBeNull();

            var scriptBlock = ScriptBlock.Create(Sut.ToScriptBlock(extensionMethod));
            var output = scriptBlock.Invoke().Single().BaseObject;

            output.Should().BeOfType<string>();
            var str = output as string;
            str.Should().Be($"Hello from {nameof(ExampleClassExtensions.ExtensionProperty)}");
        }

        [Fact]
        public void Reference_ShouldBeCallable()
        {
            var extensionMethod = typeof(ExampleClassExtensions).GetMethod(nameof(ExampleClassExtensions.ExtensionReference));
            extensionMethod.Should().NotBeNull();

            var scriptBlock = ScriptBlock.Create(Sut.ToScriptBlock(extensionMethod));
            var output = scriptBlock.Invoke(nameof(Reference_ShouldBeCallable)).Single().BaseObject;

            output.Should().BeOfType<string>();
            var str = output as string;
            str.Should().Be($"Hello {nameof(Reference_ShouldBeCallable)} from {nameof(ExampleClassExtensions.ExtensionReference)}");
        }
    }
}
