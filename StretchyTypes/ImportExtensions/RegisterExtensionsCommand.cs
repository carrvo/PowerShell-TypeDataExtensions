using ImportExtensions.TypeConverters;
using ImportExtensions.Validation;
using System;
using System.ComponentModel;
using System.Management.Automation.Runspaces;
using System.Reflection;

namespace ImportExtensions
{
    /// <summary>
    /// <para type="synopsis"></para>
    /// <para type="description"></para>
    /// </summary>
    [Cmdlet(VerbsLifecycle.Register, "Extensions")]
    [Alias("Expand-Extensions")]
    public sealed class RegisterExtensionsCommand : PSCmdlet
    {
        internal const String ExtensionErrorId = "Register Extension Error";

        /// <summary>
        /// <para type="synopsis"></para>
        /// </summary>
        [Parameter(Mandatory = true)]
        [ValidateGenericType]
        //[TypeConverter(typeof(TypeDefinitionConverter))] // only works when attributed on classes
        public TypeDelegator Generic { get; set; }

        /// <summary>
        /// <para type="synopsis"></para>
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public Type[] Specific { get; set; }

        private Dictionary<String, Object> InvocationParameters { get; set; }
        private TypeData GenericTypeData { get; set; }

        /// <inheritdoc/>
        protected override void BeginProcessing()
        {
            InvocationParameters = MyInvocation.BoundParameters.ToDictionary(entry => entry.Key, entry => entry.Value);
            InvocationParameters.Remove(nameof(Generic));
            InvocationParameters.Remove(nameof(Specific));

            GenericTypeData = InvokeCommand.InvokeScript(@"
                    Param($GenericType, $Bound)
                    Get-TypeData @Bound |
                        Where-Object TypeName -EQ $GenericType.FullName
                ", Generic, InvocationParameters)
                .FirstOrDefault()
                ?.BaseObject as TypeData;

            if (GenericTypeData is null)
            {
                ThrowTerminatingError(new ErrorRecord(new ArgumentException("No TypeData found!"), ExtensionErrorId, ErrorCategory.InvalidArgument, Generic));
            }
        }

        /// <inheritdoc/>
        protected override void ProcessRecord()
        {
            Type newType = default;
            try
            {
                WriteVerbose($"Determining type from `{Generic}` combined with: `{String.Join("`, `", Specific.Select(x => x.ToString()))}`");
                newType = Generic.MakeGenericType(Specific);
                if (newType is null)
                {
                    WriteError(new ErrorRecord(new ArgumentException("Could not generate specific type!"), ExtensionErrorId, ErrorCategory.InvalidArgument, Specific));
                    return;
                }
                WriteVerbose($"Creating TypeData for: `{newType}`");

                foreach (var memberdata in GenericTypeData
                    .Members
                    .Where(x => x.Value.GetType() == typeof(ScriptMethodData))
                    .Select(x => (Name: x.Key, Data: x.Value as ScriptMethodData)))
                {
                    WriteVerbose($"Update-TypeData -TypeName {newType} -MemberType ScriptMethod -MemberName {memberdata.Name} -Value {{{memberdata.Data?.Script}}} {String.Join(" ", InvocationParameters.Select(x => $"-{x.Key} {x.Value}"))}");
                    InvokeCommand.InvokeScript(@"
    Param($ParameterType, $Name, $ScriptBlock, $Bound)
    Update-TypeData -TypeName $ParameterType.ToString() -MemberType ScriptMethod -MemberName $Name -Value $ScriptBlock @Bound
", newType, memberdata.Name, memberdata.Data?.Script, InvocationParameters);
                }
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, ExtensionErrorId, ErrorCategory.NotSpecified, newType));
            }
        }
    }
}
