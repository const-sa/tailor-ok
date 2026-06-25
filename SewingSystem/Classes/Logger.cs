using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace SewingSystem.Classes
{
    /// <summary>
    /// Lightweight error logger. Exceptions that used to be swallowed by empty
    /// catch blocks are now recorded here, so failures are visible during
    /// development/diagnosis without changing runtime behaviour (callers still
    /// continue as before). Logging itself never throws.
    /// </summary>
    public static class Logger
    {
        private static readonly object _sync = new object();

        private static readonly string LogPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sewing-errors.log");

        public static void Log(Exception ex, [CallerMemberName] string source = "")
        {
            try
            {
                string header = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}  [{source}]  " +
                                $"{ex?.GetType().Name}: {ex?.Message}";
                Debug.WriteLine(header);
                lock (_sync)
                {
                    File.AppendAllText(LogPath,
                        header + Environment.NewLine +
                        (ex?.StackTrace ?? string.Empty) + Environment.NewLine + Environment.NewLine);
                }
            }
            catch
            {
                // A logger must never crash the application.
            }
        }
    }
}
