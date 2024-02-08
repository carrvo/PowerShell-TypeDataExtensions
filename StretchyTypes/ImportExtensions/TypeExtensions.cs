using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ImportExtensions
{
    internal static class TypeExtensions
    {
        private static Regex GenericTypeTrimmer { get; } = new Regex(@"(?<depth>`\d+)\[");
        private static Regex GenericTypeNameTrimmer { get; } = new Regex(@"(?<depth>`\d+)$");

        internal static String ToPSType(this Type type)
        {
            if (type.IsByRef)
            {
                return "ref";
            }

            if (type.IsGenericParameter)
            {
                type = type.BaseType;
            }

            if (type.IsGenericType)
            {
                var typeStr = type.ToString();
                return GenericTypeTrimmer.Replace(typeStr, "[");
            }

            return type.FullName;
        }

        internal static String ToRecursivePSType(this Type type)
        {
            if (type.IsByRef)
            {
                return "ref";
            }

            if (type.IsGenericParameter)
            {
                type = type.BaseType;
            }

            if (type.IsGenericType)
            {
                var typeStr = $"{type.Namespace}.{GenericTypeNameTrimmer.Replace(type.Name, String.Empty)}";
                var genericArguments = type.GenericTypeArguments.Select(x => x.ToRecursivePSType());
                return $"{typeStr}[{String.Join(",", genericArguments)}]";
            }

            return type.FullName;
        }
    }
}
