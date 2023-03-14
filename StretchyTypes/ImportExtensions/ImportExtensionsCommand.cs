﻿using System;
using System.Management.Automation;
using System.Reflection;

namespace ImportExtensions
{
    /// <summary>
    /// <para type="synopsis"></para>
    /// <para type="description"></para>
    /// </summary>
    [Cmdlet(VerbsData.Import, "Extensions", ConfirmImpact = ConfirmImpact.High, RemotingCapability = RemotingCapability.None)]
    public sealed class ImportExtensionsCommand : PSCmdlet
    {
        /// <summary>
        /// <para type="synopsis"></para>
        /// </summary>
        [Parameter(Position = 0, Mandatory = false, ValueFromPipeline = true)]
        Assembly Assembly { get; set; } = Assembly.GetExecutingAssembly();

        protected override void ProcessRecord()
        {
            IEnumerable<Type> staticClasses = FindClasses(Assembly)
                .Where(x => IsStaticClass(x));
            IEnumerable<MethodInfo> extensionMethods = staticClasses
                .SelectMany(x => x.GetMethods()) //x.GetRuntimeMethods() ??
                .Where(x => IsExtensionMethod(x));
                //.GroupBy(x => ExtendsType(x));
            
            foreach (MethodInfo extension in extensionMethods)
            {
                InvokeCommand.InvokeScript(@"
    Update-TypeData
");
            }
        }

        private bool IsExtensionMethod(MethodInfo x)
        {
            throw new NotImplementedException();
        }

        private bool IsStaticClass(Type x)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Type> FindClasses(Assembly assembly)
        {
            throw new NotImplementedException();
        }
    }
}