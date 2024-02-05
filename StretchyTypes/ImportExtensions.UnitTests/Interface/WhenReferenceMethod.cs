using FluentAssertions;
using Xunit;

namespace ImportExtensions.UnitTests.Interface
{
    public sealed class WhenReferenceMethod
    {
        [Fact]
        public void ShouldNotBeExtension()
        {
            ImportExtensionsCommand.IsExtensionMethod(typeof(ExampleClass).GetMethod(nameof(IExampleClass.Method))).Should().BeFalse();
        }
    }
}
