namespace MachineInformationApp.Interfaces
{
    public interface IScriptExecutor
    {
        ScriptOutput ExecutePowershell(string scriptText);
    }
}