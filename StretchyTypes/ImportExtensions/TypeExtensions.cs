using System;
using System.Text.RegularExpressions;

namespace ImportExtensions
{
    internal static class TypeExtensions
    {
        private static Regex GenericTypeTrimmer { get; } = new Regex(@"(?<depth>`\d+)\[");

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
    }
}
