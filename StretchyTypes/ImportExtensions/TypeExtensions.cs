using System;
using System.Text;
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
            var typeStr = new StringBuilder();
            void ConvertToPSType(Type convert)
            {
                if (convert.IsByRef)
                {
                    typeStr.Append("ref");
                    return;
                }

                if (convert.IsGenericParameter)
                {
                    convert = convert.BaseType;
                }

                if (convert.IsGenericType)
                {
                    typeStr.Append(convert.Namespace);
                    typeStr.Append(".");
                    typeStr.Append(GenericTypeNameTrimmer.Replace(convert.Name, String.Empty));
                    typeStr.Append("[");
                    foreach (var genericArgument in convert.GenericTypeArguments)
                    {
                        ConvertToPSType(genericArgument);
                        typeStr.Append(",");
                    }
                    typeStr.Remove(typeStr.Length - 1, 1); // remove final comma `,`
                    typeStr.Append("]");
                    return;
                }

                typeStr.Append(convert.FullName);
            }
            ConvertToPSType(type);
            return typeStr.ToString();
        }
    }
}
