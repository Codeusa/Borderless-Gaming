using System.Diagnostics;

namespace BorderlessGaming.Logic.Extensions
{
    internal static class ProcessExtensions
    {

#nullable enable
        public static Process? GetProcessById(int id)
        {
            try
            {
                return Process.GetProcessById(id);
            }
            catch
            {
                return null;
            }
        }
#nullable disable

        private static string FindIndexedProcessName(int pid)
        {
            using var process = GetProcessById(pid);
            if (process is null)
            {
                return string.Empty;
            }
            var processName = process.ProcessName;
            var processesByName = Process.GetProcessesByName(processName);
            string processIndexdName = null;

            for (var index = 0; index < processesByName.Length; index++)
            {
                processIndexdName = index == 0 ? processName : processName + "#" + index;
                var processId = new PerformanceCounter("Process", "ID Process", processIndexdName);
                if ((int)processId.NextValue() == pid)
                {
                    return processIndexdName;
                }
            }

            return processIndexdName;
        }

        private static Process FindPidFromIndexedProcessName(string indexedProcessName)
        {
            if (string.IsNullOrEmpty(indexedProcessName))
            {
                return null;
            }
            var parentId = new PerformanceCounter("Process", "Creating Process ID", indexedProcessName);
            return Process.GetProcessById((int)parentId.NextValue());
        }

        public static Process Parent(this Process process)
        {
            return FindPidFromIndexedProcessName(FindIndexedProcessName(process.Id));
        }
    }
}