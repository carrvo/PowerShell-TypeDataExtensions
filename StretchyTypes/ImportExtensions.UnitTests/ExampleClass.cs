using System;

namespace ImportExtensions.UnitTests
{
    public sealed class ExampleClass
    {
        public String Method(String name)
        {
            return $"Hello {name} from {nameof(Method)}";
        }

        public static String StaticMethod(String name)
        {
            return $"Hello {name} from {nameof(StaticMethod)}";
        }
    }

    public static class ExampleClassExtensions
    {
        public static String StaticMethod(String name)
        {
            return $"Hello {name} from {nameof(StaticMethod)}";
        }

        public static String ExtensionMethod(this String name)
        {
            return $"Hello {name} from {nameof(ExtensionMethod)}";
        }
    }
}
