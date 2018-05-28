using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation;
using MachineInformationApp.Interfaces;

namespace MachineInformationApp
{
    public class ScriptExecutor : IScriptExecutor
    {
        public ScriptOutput ExecutePowershell(string scriptText)
        {
            var results = string.Empty;
            var executionOutput = string.Empty;
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                PowerShellInstance.AddScript(scriptText);
                Collection<PSObject> PSOutput = null;
                try
                {
                    PSOutput = PowerShellInstance.Invoke();
                    if (HasErrors(PowerShellInstance))
                    {
                        var errorMessage = string.Empty;
                        foreach (var errorRecord in PowerShellInstance.Streams.Error)
                        {
                            errorMessage += errorRecord;
                        }
                        executionOutput = errorMessage;
                        return new ScriptOutput { Output = executionOutput, ExitCode = 1 };
                    }
                    executionOutput = GetListOfFileContents(results, PSOutput);
                    return new ScriptOutput { Output = executionOutput, ExitCode = 0 };

                }
                catch (Exception e)
                {
                    return new ScriptOutput { Output = e.Message, ExitCode = 1 };
                }
            }
        }


        private static bool HasErrors(PowerShell PowerShellInstance)
        {
            return PowerShellInstance.Streams.Error.Count > 0;
        }

        private static string GetListOfFileContents(string results, Collection<PSObject> PSOutput)
        {
            foreach (PSObject outputItem in PSOutput)
            {
                results += AppendItemsInList(results, outputItem);
            }
            return results;
        }

        private static string AppendItemsInList(string results, PSObject outputItem)
        {
            if (outputItem != null)
            {
                results = outputItem.ToString();
            }
            return results;
        }
    }
}