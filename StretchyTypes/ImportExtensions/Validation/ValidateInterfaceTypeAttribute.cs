using System;
using System.Management.Automation;

namespace ImportExtensions.Validation
{
    /// <inheritdoc/>
    internal sealed class ValidateInterfaceTypeAttribute : ValidateArgumentsAttribute
    {
        /// <inheritdoc/>
        protected override void Validate(object arguments, EngineIntrinsics engineIntrinsics)
        {
            if (arguments is Type type)
            {
                if (type.IsInterface)
                {
                    return;
                }
                throw new ValidationMetadataException($"Type `{type}` is not an interface type.");
            }
            throw new ValidationMetadataException($"Argument `{arguments}` is not a type.");
        }
    }
}
