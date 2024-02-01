using System;
using System.Management.Automation;

namespace ImportExtensions.Validation
{
    internal sealed class ValidateGenericTypeAttribute : ValidateArgumentsAttribute
    {
        /// <inheritdoc/>
        protected override void Validate(object arguments, EngineIntrinsics engineIntrinsics)
        {
            if (arguments is Type type)
            {
                if (type.IsGenericType)
                {
                    return;
                }
                throw new ValidationMetadataException($"Type `{type}` is not a generic type.");
            }
            throw new ValidationMetadataException($"Argument `{arguments}` is not a type.");
        }
    }
}
