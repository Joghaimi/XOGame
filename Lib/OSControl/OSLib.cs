using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.OSControl
{
    public static class OSLib
    {
        public static void ResetService(string serviceName)
        {
            try
            {
                ExecuteShellCommand($"sudo systemctl stop {serviceName}");
                ExecuteShellCommand($"sudo systemctl start {serviceName}");
                Console.WriteLine($"Service '{serviceName}' has been reset.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while resetting the service '{serviceName}': {ex.Message}");
            }
        }

        private static void ExecuteShellCommand(string command)
        {
            Process process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{command}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            // Wait for the process to exit
            process.WaitForExit();

            // Check for errors
            if (!string.IsNullOrEmpty(error))
            {
                throw new Exception($"Error executing command: {error}");
            }
        }
    }
}
