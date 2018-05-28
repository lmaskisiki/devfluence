using System;

namespace MachineInformationApp
{
    public class OSGenerator : IOSGenerator
    {
        public ExecutionOutput GetOsVersion()
        {
            return new ExecutionOutput         { result = Environment.OSVersion.ToString() };
        }
    }
}