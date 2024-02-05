using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace ImportExtensions.UnitTests
{
    public static class RunspaceWrapper
    {
        public static Runspace RunspaceExecution { get; private set; }

        static RunspaceWrapper()
        {
            SetDefaultRunspace();
        }

        public static void SetDefaultRunspace()
        {
            RunspaceExecution = RunspaceFactory.CreateRunspace();
            RunspaceExecution.Open();
            Runspace.DefaultRunspace = RunspaceExecution;
        }

#if NET7_0_OR_GREATER
        public static Collection<PSObject> RunspaceExecute(String command, params Object[] input)
        {
            var pipeline = RunspaceExecution.CreatePipeline(command);
            return pipeline.Invoke(input);
        }
#endif
    }
}
