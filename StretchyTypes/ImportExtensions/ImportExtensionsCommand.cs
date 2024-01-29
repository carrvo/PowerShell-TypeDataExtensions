using System;
using System.Management.Automation;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

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
                var scriptBlock = InvokeCommand.NewScriptBlock(ToScriptBlock(extension));
                var parameterType = extension.GetParameters().First().ParameterType;
                WriteVerbose($"Update-TypeData -TypeName {parameterType.FullName} -MemberType ScriptMethod -MemberName {extension.Name} -Value {{{scriptBlock}}} -ErrorAction Stop");
                InvokeCommand.InvokeScript(@"
    Param($ParameterType, $StaticMethod, $ScriptBlock)
    Update-TypeData -TypeName $ParameterType.FullName -MemberType ScriptMethod -MemberName $StaticMethod.Name -Value $ScriptBlock -ErrorAction Stop
", parameterType, extension, scriptBlock);
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

        internal String ToScriptBlock(MethodInfo staticMethod)
        {
            StringBuilder command = new StringBuilder();
            IList<String> arguments = new List<String>();

            command.AppendLine("Param(");
            foreach (var parameterInfo in staticMethod.GetParameters().Skip(1))
            {
                command.AppendLine($"  [{parameterInfo.ParameterType.FullName}] ${parameterInfo.Name},");
                arguments.Add($"${parameterInfo.Name}");
            }
            command.Remove(command.Length - Environment.NewLine.Length - 1, 1); // remove final comma `,`
            command.AppendLine(")");

            command.Append($"[{staticMethod.DeclaringType?.FullName}]::{staticMethod.Name}");
            command.Append("(");
            command.Append("$this");
            if (arguments.Any())
            {
                command.Append(", ");
                command.AppendJoin(", ", arguments);
            }
            command.Append(")");

            return command.ToString();
        }
    }
}
