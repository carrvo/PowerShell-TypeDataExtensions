using ImportExtensions.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reflection;

namespace ImportExtensions
{
    /// <summary>
    /// <para type="synopsis"></para>
    /// <para type="description"></para>
    /// </summary>
    [Cmdlet(VerbsCommon.Copy, "Extensions")]
    [Alias("Register-Extensions")]
    public sealed class CopyExtensionsCommand : PSCmdlet
    {
        internal const String ExtensionErrorId = "Register Extension Error";

        /// <summary>
        /// <para type="synopsis"></para>
        /// </summary>
        [Parameter(Mandatory = true)]
        public Type From { get; set; }

        /// <summary>
        /// <para type="synopsis"></para>
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public Type To { get; set; }

        private Dictionary<String, Object> InvocationParameters { get; set; }
        private TypeData FromTypeData { get; set; }

        /// <inheritdoc/>
        protected override void BeginProcessing()
        {
            InvocationParameters = MyInvocation.BoundParameters.ToDictionary(entry => entry.Key, entry => entry.Value);
            InvocationParameters.Remove(nameof(From));
            InvocationParameters.Remove(nameof(To));

            FromTypeData = this.GetTypeData(From, InvocationParameters);

            if (FromTypeData is null)
            {
                ThrowTerminatingError(new ErrorRecord(new ArgumentException("No TypeData found!"), ExtensionErrorId, ErrorCategory.InvalidArgument, From));
            }
        }

        /// <inheritdoc/>
        protected override void ProcessRecord()
        {
            try
            {
                WriteVerbose($"Creating TypeData for: `{To}`");
                WriteWarning($"No validation is done to see if the extensions from `{From}` applies to `{To}`!");

                foreach (var memberdata in FromTypeData
                    .Members
                    .Where(x => x.Value.GetType() == typeof(ScriptMethodData))
                    .Select(x => (Name: x.Key, Data: x.Value as ScriptMethodData)))
                {
                    this.UpdateTypeData(To, memberdata.Name, memberdata.Data?.Script, InvocationParameters);
                }
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, ExtensionErrorId, ErrorCategory.NotSpecified, To));
            }
        }
    }
}
