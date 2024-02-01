using System;
using System.Management.Automation;
using System.Reflection;
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
    public sealed class TypeDelegatorConverter : PSTypeConverter
    {
        private Regex BuiltInConverter = new Regex(@"^\[(?<type>.*)\]$");

        /// <inheritdoc/>
        public override bool CanConvertFrom(object sourceValue, Type destinationType)
            => destinationType == typeof(TypeDelegator);

        /// <inheritdoc/>
        public override bool CanConvertTo(object sourceValue, Type destinationType)
            => throw new NotImplementedException();

        /// <inheritdoc/>
        public override object ConvertFrom(object sourceValue, Type destinationType, IFormatProvider formatProvider, bool ignoreCase)
        {
            if (sourceValue is Type type)
            {
                return new GenericType(type);
            }
            else if (sourceValue is String typeName)
            {
                var shortName = typeName.Split(".").Last();
                var builtin = BuiltInConverter.Match(typeName);
                if (builtin.Success)
                {
                    typeName = builtin.Groups["type"].Value;
                }
                Regex typeRegex = new Regex(typeName);
                var potentialTypes = AppDomain
                    .CurrentDomain
                    .GetAssemblies()
                    .SelectMany(assembly => assembly.ExportedTypes)
                    .Where(type => type.ToString() == typeName || typeRegex.IsMatch(type.ToString()));
                // find best match
                var bestMatch = potentialTypes.FirstOrDefault(type => type.Name == shortName) ?? potentialTypes.First();
                return new GenericType(bestMatch);
            }
            else
            {
                return new GenericType(sourceValue.GetType());
            }
        }

        /// <inheritdoc/>
        public override object ConvertTo(object sourceValue, Type destinationType, IFormatProvider formatProvider, bool ignoreCase)
            => throw new NotImplementedException();
    }
}
