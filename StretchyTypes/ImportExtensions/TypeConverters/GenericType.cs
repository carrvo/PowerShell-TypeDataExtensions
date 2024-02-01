using System;
using System.Reflection;

namespace ImportExtensions.TypeConverters
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class GenericType : TypeDelegator
    {
        /// <inheritdoc/>
        public GenericType(Type type)
            : base(type)
        { }

        /// <inheritdoc/>
        public override bool IsGenericType => base.typeImpl.IsGenericType;

        /// <inheritdoc/>
        public override string ToString() => base.typeImpl.ToString();
    }
}
