namespace ImportExtensions.UnitTests.Generic
{
    public interface IExampleClass<T>
    {
        string Method(string name);
    }
    public sealed class ExampleClass<T> : IExampleClass<T>
    {
        public string Method(string name)
        {
            return $"Hello {name} from {nameof(Method)} with {typeof(T).Name}";
        }

        public static string StaticMethod(string name)
        {
            return $"Hello {name} from {nameof(StaticMethod)} with {typeof(T).Name}";
        }
    }

    public static class ExampleStatic<T>
    {
        public static string StaticMethod(ExampleClass<T> example, string name)
        {
            return $"Hello {name} from {nameof(StaticMethod)} with {typeof(T).Name}";
        }
    }

    public static class ExampleClassExtensions
    {
        public static string StaticMethod<T>(ExampleClass<T> example, string name)
        {
            return $"Hello {name} from {nameof(StaticMethod)} with {typeof(T).Name}";
        }

        public static string StaticIMethod<T>(IExampleClass<T> example, string name)
        {
            return $"Hello {name} from {nameof(StaticIMethod)} with {typeof(T).Name}";
        }

        public static string ExtensionMethod<T>(this ExampleClass<T> example, string name)
        {
            return $"Hello {name} from {nameof(ExtensionMethod)} with {typeof(T).Name}";
        }

        public static string ExtensionIMethod<T>(this IExampleClass<T> example, string name)
        {
            return $"Hello {name} from {nameof(ExtensionIMethod)} with {typeof(T).Name}";
        }
    }
}
