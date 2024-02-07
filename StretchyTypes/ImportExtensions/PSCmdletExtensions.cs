using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Text;

namespace ImportExtensions
{
    internal static class PSCmdletExtensions
    {
        internal static void UpdateTypeData(this PSCmdlet invocation, Type parameterType, String extension, ScriptBlock scriptBlock, IDictionary<String, Object> bound)
        {
            invocation.WriteVerbose($"Update-TypeData -TypeName {parameterType} -MemberType ScriptMethod -MemberName {extension} -Value {{{scriptBlock}}} {String.Join(" ", bound.Select(x => $"-{x.Key} {x.Value}"))}");
            switch (invocation.CommandRuntime.Host.Version.Major)
            {
                case 5:
                    // PowerShell 5 does not handle passing arguments to InvokeScript nicely (they seem to be null)
                    invocation.InvokeCommand.InvokeScript($@"
                        Update-TypeData -TypeName {parameterType} -MemberType ScriptMethod -MemberName {extension} -Value {{{scriptBlock.ToString().Replace(Environment.NewLine, String.Empty)}}}
                    ");
                    // create second "dummy" type (for generics) such that it can be referenced differently
                    invocation.InvokeCommand.InvokeScript($@"
                        Update-TypeData -TypeName {parameterType.ToString().Replace("`1", String.Empty)} -MemberType ScriptMethod -MemberName {extension} -Value {{{scriptBlock.ToString().Replace(Environment.NewLine, String.Empty)}}}
                    ");
                    return;
                default:
                    invocation.InvokeCommand.InvokeScript(@"
                        Param($ParameterType, $StaticMethod, $ScriptBlock, $Bound)
                        Update-TypeData -TypeName $ParameterType.ToString() -MemberType ScriptMethod -MemberName $StaticMethod -Value $ScriptBlock @Bound
                    ", parameterType, extension, scriptBlock, bound);
                    return;
            }
        }

        internal static TypeData GetTypeData(this PSCmdlet invocation, Type parameterType, IDictionary<String, Object> bound)
        {
            invocation.WriteVerbose($"Finding TypeData for: `{parameterType.FullName}`");
            switch (invocation.CommandRuntime.Host.Version.Major)
            {
                case 5:
                    // PowerShell 5 does not handle passing arguments to InvokeScript nicely (they seem to be null)
                    return invocation.InvokeCommand.InvokeScript($@"
                        Param($GenericType, $Bound)
                        Get-TypeData |
                            Where-Object TypeName -EQ {parameterType.FullName}
                    ")
                    .FirstOrDefault()
                    ?.BaseObject as TypeData;
                default:
                    return invocation.InvokeCommand.InvokeScript(@"
                        Param($GenericType, $Bound)
                        Get-TypeData @Bound |
                           Where-Object TypeName -EQ $GenericType.FullName
                    ", parameterType, bound)
                    .FirstOrDefault()
                    ?.BaseObject as TypeData;
            }
        }
    }
}
