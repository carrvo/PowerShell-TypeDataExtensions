using System.Management.Automation;

namespace ImportExtensions.UnitTests.Generic
{
    public sealed class WhenConvertingToScriptBlock
    {
        public MethodInfo? ExtensionMethod { get; set; }
        public ImportExtensionsCommand Sut { get; }
        public ExampleClass<int> Input { get; }

        public WhenConvertingToScriptBlock()
        {
            ExtensionMethod = typeof(ExampleClassExtensions).GetMethod(nameof(ExampleClassExtensions.ExtensionMethod));
            RunspaceWrapper.SetDefaultRunspace();

            RunspaceWrapper.RunspaceExecution.Should().NotBeNull();
            ExtensionMethod.Should().NotBeNull();

            Sut = new ImportExtensionsCommand();
            Input = new ExampleClass<int>();
        }

        [Fact]
        public void ShouldBeCallable()
        {
            var scriptBlock = ScriptBlock.Create(Sut.ToScriptBlock(ExtensionMethod));
            var output = scriptBlock.Invoke(nameof(ShouldBeCallable)).Single().BaseObject;

            output.Should().BeOfType<string>();
            var str = output as string;
            str.Should().Be($"Hello {nameof(ShouldBeCallable)} from {nameof(ExampleClassExtensions.ExtensionMethod)} with {typeof(int).Name}");
        }
    }
}
