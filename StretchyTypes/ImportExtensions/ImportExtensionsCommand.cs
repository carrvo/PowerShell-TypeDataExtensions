using System;
using System.Management.Automation;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("ImportExtensions.UnitTests")]
namespace ImportExtensions
{
    /// <summary>
    /// <para type="synopsis"></para>
    /// <para type="description"></para>
    /// </summary>
    [Cmdlet(VerbsData.Import, "Extensions", DefaultParameterSetName = "assembly", ConfirmImpact = ConfirmImpact.High, RemotingCapability = RemotingCapability.None)]
    public sealed class ImportExtensionsCommand : PSCmdlet
    {
        /// <summary>
        /// <para type="synopsis"></para>
        /// </summary>
        [Parameter(Position = 0, Mandatory = false, ValueFromPipeline = true, ParameterSetName = "assembly")]
        public Assembly Assembly { get; set; } = Assembly.GetExecutingAssembly();

        /// <summary>
        /// <para type="synopsis"></para>
        /// </summary>
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ParameterSetName = "path")]
        public FileInfo Path { get; set; } = null;

        /// <inheritdoc/>
        protected override void ProcessRecord()
        {
            if (Path != null)
            {
                Assembly = Assembly.LoadFrom(Path.FullName);
            }

            IEnumerable<Type> staticClasses = Assembly.GetExportedTypes()
                .Where(type => IsStaticClass(type));
            IEnumerable<MethodInfo> extensionMethods = staticClasses
                .SelectMany(type => type.GetMethods()) //type.GetRuntimeMethods() ??
                .Where(method => IsExtensionMethod(method));
                //.GroupBy(method => ExtendsType(method));
            
            foreach (MethodInfo extension in extensionMethods)
            {
                InvokeCommand.InvokeScript(@"
    Param($ParameterType, $StaticMethod)
    Update-TypeData -TypeName $ParameterType.Name -MemberType CodeMethod -MemberName $StaticMethod.Name -Value $StaticMethod -ErrorAction Stop
", extension.GetParameters().First().ParameterType, extension);
            }
        }

        /// <remarks>
        /// Kudos to https://stackoverflow.com/a/299526
        /// </remarks>
        internal static bool IsExtensionMethod(MethodInfo method)
        {
            return method.IsStatic && method.IsDefined(typeof(ExtensionAttribute), false);
        }

        internal static bool IsStaticClass(Type type)
        {
            return type.IsDefined(typeof(ExtensionAttribute), false); // type.GetConstructors().Length == 0;
        }
    }
}
