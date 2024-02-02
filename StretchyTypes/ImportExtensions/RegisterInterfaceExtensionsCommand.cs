using ImportExtensions.Validation;
using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reflection;

namespace ImportExtensions
{
    /// <summary>
    /// <para type="synopsis"></para>
    /// <para type="description"></para>
    /// </summary>
    [Cmdlet(VerbsLifecycle.Register, "InterfaceExtensions")]
    [Alias("Expand-InterfaceExtensions")]
    public sealed class RegisterInterfaceExtensionsCommand : PSCmdlet
    {
        internal const String ExtensionErrorId = "Register Extension Error";

        /// <summary>
        /// <para type="synopsis"></para>
        /// </summary>
        [Parameter(Mandatory = true)]
        [ValidateInterfaceType]
        public Type Interface { get; set; }

        /// <summary>
        /// <para type="synopsis"></para>
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public Type Concrete { get; set; }

        private Dictionary<String, Object> InvocationParameters { get; set; }
        private TypeData InterfaceTypeData { get; set; }

        /// <inheritdoc/>
        protected override void BeginProcessing()
        {
            InvocationParameters = MyInvocation.BoundParameters.ToDictionary(entry => entry.Key, entry => entry.Value);
            InvocationParameters.Remove(nameof(Interface));
            InvocationParameters.Remove(nameof(Concrete));

            WriteVerbose($"Finding TypeData for: `{Interface.FullName}`");
            InterfaceTypeData = InvokeCommand.InvokeScript(@"
                    Param($GenericType, $Bound)
                    Get-TypeData @Bound |
                        Where-Object TypeName -EQ $GenericType.FullName
                ", Interface, InvocationParameters)
                .FirstOrDefault()
                ?.BaseObject as TypeData;

            if (InterfaceTypeData is null)
            {
                ThrowTerminatingError(new ErrorRecord(new ArgumentException("No TypeData found!"), ExtensionErrorId, ErrorCategory.InvalidArgument, Interface));
            }
        }

        /// <inheritdoc/>
        protected override void ProcessRecord()
        {
            try
            {
                WriteVerbose($"Creating TypeData for: `{Concrete}`");

                foreach (var memberdata in InterfaceTypeData
                    .Members
                    .Where(x => x.Value.GetType() == typeof(ScriptMethodData))
                    .Select(x => (Name: x.Key, Data: x.Value as ScriptMethodData)))
                {
                    WriteVerbose($"Update-TypeData -TypeName {Concrete} -MemberType ScriptMethod -MemberName {memberdata.Name} -Value {{{memberdata.Data?.Script}}} {String.Join(" ", InvocationParameters.Select(x => $"-{x.Key} {x.Value}"))}");
                    InvokeCommand.InvokeScript(@"
    Param($ParameterType, $Name, $ScriptBlock, $Bound)
    Update-TypeData -TypeName $ParameterType.ToString() -MemberType ScriptMethod -MemberName $Name -Value $ScriptBlock @Bound
", Concrete, memberdata.Name, memberdata.Data?.Script, InvocationParameters);
                }
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, ExtensionErrorId, ErrorCategory.NotSpecified, Concrete));
            }
        }
    }
}
