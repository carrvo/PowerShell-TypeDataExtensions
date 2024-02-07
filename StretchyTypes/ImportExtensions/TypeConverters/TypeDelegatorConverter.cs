using System;
using System.Linq;
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
                var shortName = typeName.Split('.').Last();
                if (BuiltInConverter.IsMatch(typeName))
                {
                    throw new ArgumentException($"Do not wrap in []: {typeName}");
                }
                Regex typeRegex = new Regex(typeName);
                var potentialTypes = AppDomain
                    .CurrentDomain
                    .GetAssemblies()
                    // Kudos to https://stackoverflow.com/a/43675843
                    .Where(p => !p.IsDynamic) // Needed for PowerShell 5
                    .SelectMany(assembly => assembly.ExportedTypes)
                    .Where(expType => expType.ToString() == typeName || typeRegex.IsMatch(expType.ToString()));
                // find best match
                var bestMatch = potentialTypes.FirstOrDefault(potType => potType.Name == shortName) ?? potentialTypes.First();
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
