using System;
using System.Management.Automation;
using System.Text.RegularExpressions;

namespace ImportExtensions.TypeConverters
{
    /// <summary>
    /// Converts to the first matching <see cref="Type"/> found.
    /// 
    /// It will either:
    /// - literal or <see cref="Regex"/> search for the loaded type using the name (<see cref="Type.ToString"/>)
    /// - use the type of the source
    /// - pass through if the source is a type
    /// </summary>
    public sealed class TypeDefinitionConverter : PSTypeConverter
    {
        /// <inheritdoc/>
        public override bool CanConvertFrom(object sourceValue, Type destinationType)
            => destinationType == typeof(Type);

        /// <inheritdoc/>
        public override bool CanConvertTo(object sourceValue, Type destinationType)
            => throw new NotImplementedException();

        /// <inheritdoc/>
        public override object ConvertFrom(object sourceValue, Type destinationType, IFormatProvider formatProvider, bool ignoreCase)
        {
            if (sourceValue is Type type)
            {
                return type;
            }
            else if (sourceValue is String typeName)
            {
                Regex typeRegex = new Regex(typeName);
                return AppDomain
                    .CurrentDomain
                    .GetAssemblies()
                    .SelectMany(assembly => assembly.ExportedTypes)
                    .First(type => type.ToString() == typeName || typeRegex.IsMatch(type.ToString()));
            }
            else
            {
                return sourceValue.GetType();
            }
        }

        /// <inheritdoc/>
        public override object ConvertTo(object sourceValue, Type destinationType, IFormatProvider formatProvider, bool ignoreCase)
            => throw new NotImplementedException();
    }
}
