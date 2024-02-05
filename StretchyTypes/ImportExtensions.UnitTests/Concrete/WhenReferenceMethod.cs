using FluentAssertions;
using Xunit;

namespace ImportExtensions.UnitTests.Concrete
{
    public sealed class WhenReferenceMethod
    {
        [Fact]
        public void ShouldNotBeExtension()
        {
            ImportExtensionsCommand.IsExtensionMethod(typeof(ExampleClass).GetMethod(nameof(ExampleClass.Method))).Should().BeFalse();
        }
    }
}
