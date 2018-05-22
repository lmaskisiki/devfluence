using System;

namespace MachineInformationApp
{
    public class OSGenerator : IOSGenerator
    {
        public ExecutionOutput GetOsVersion()
        {
            return new ExecutionOutput         { Output = Environment.OSVersion.ToString() };
        }
    }
}