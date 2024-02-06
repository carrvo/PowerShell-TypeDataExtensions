using FluentAssertions;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using Xunit;

namespace ImportExtensions.UnitTests.Interface
{
#if !SKIP_TESTS
    public sealed class WhenConvertingToScriptBlock
    {
        public MethodInfo ExtensionMethod { get; set; }
        public ImportExtensionsCommand Sut { get; }
        public IExampleClass Input { get; }

        public WhenConvertingToScriptBlock()
        {
            ExtensionMethod = typeof(ExampleClassExtensions).GetMethod(nameof(ExampleClassExtensions.ExtensionIMethod));
            RunspaceWrapper.SetDefaultRunspace();

            RunspaceWrapper.RunspaceExecution.Should().NotBeNull();
            ExtensionMethod.Should().NotBeNull();

            Sut = new ImportExtensionsCommand();
            Input = new ExampleClass();
        }

        [Fact]
        public void ShouldBeCallable()
        {
            var scriptBlock = ScriptBlock.Create(Sut.ToScriptBlock(ExtensionMethod));
            var output = scriptBlock.Invoke(nameof(ShouldBeCallable)).Single().BaseObject;

            output.Should().BeOfType<string>();
            var str = output as string;
            str.Should().Be($"Hello {nameof(ShouldBeCallable)} from {nameof(ExampleClassExtensions.ExtensionIMethod)}");
        }
    }
#endif
}
