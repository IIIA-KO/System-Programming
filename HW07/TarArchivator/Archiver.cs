using System;
using System.Diagnostics;

namespace TarArchivator
{
    public class Archiver
    {
        private Logger _logger;

        public event Action<string> LogUpdated;
        public event Action<string> ProgressUpdated;

        public Archiver(string logFilePath)
        {
            this._logger = new Logger(logFilePath);
        }

        public async Task CreateArchiveAsync(string tarFileName, string folderPath)
        {
            try
            {
                this._logger.WriteLog($"Starting archive process for folder '{folderPath}' at {DateTime.Now}\n");
                LogUpdated?.Invoke($"Starting archive process for folder '{folderPath}' at {DateTime.Now}\n");

                using Process process = new Process();

                process.StartInfo.FileName = "tar.exe";
                process.StartInfo.Arguments = $"-czvf {tarFileName} {folderPath}";

                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        _logger.WriteLog($"Output: {e.Data}\n");
                        LogUpdated?.Invoke($"Output: {e.Data}\n");
                        ProgressUpdated?.Invoke("In progress of archiving...");
                    }
                };

                process.ErrorDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        this._logger.WriteLog($"Output: {e.Data}\n");
                        LogUpdated?.Invoke($"Error: {e.Data}\n");
                        ProgressUpdated?.Invoke("Error happened. Look at logs!");
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    this._logger.WriteLog($"Archive process completed successfully at {DateTime.Now}\n");
                    LogUpdated?.Invoke($"Archive process completed successfully at {DateTime.Now}\n");
                    ProgressUpdated?.Invoke("Completed");
                }
                else
                {
                    string errorOutput = process.StandardError.ReadToEnd();
                    this._logger.WriteLog($"Error: {errorOutput}\n");
                    LogUpdated?.Invoke($"Error: {errorOutput}\n");
                    ProgressUpdated?.Invoke("Archiving failed!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}