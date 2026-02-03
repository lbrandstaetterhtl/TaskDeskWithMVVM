using System;
using System.IO;
using System.Text;
using TaskDesk_version2.Models;

namespace TaskDesk_version2;

public static class AppLogger
{
    private static readonly object Sync = new();
    private static readonly string LogDirectory = MainData.DataPath + @"\logs";
    private static readonly string LogFilePath = Path.Combine(
        LogDirectory,
        $"{DateTimeOffset.Now:yyyy-MM-dd_HH-mm-ss}.taskdesk.log");

    public static void Info(string message) => Write("[INFO]", message, null);

    public static void Warn(string message) => Write("[WARN]", message, null);

    public static void Error(string message, Exception? exception = null)
    {
        Write("[ERROR]", message, exception);
    }

    public static string GetLogFilePath() => LogFilePath;

    private static void Write(string level, string message, Exception? exception)
    {
        try
        {
            lock (Sync)
            {
                Directory.CreateDirectory(LogDirectory);
                var timestamp = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss.ff");
                var line = exception == null
                ? $"{timestamp} [{level}] {message}{Environment.NewLine}"
                : $"{timestamp} [{level}] {message} | {exception.GetType().Name}: {exception.Message}{Environment.NewLine}";
                
                File.AppendAllText(LogFilePath, line, Encoding.UTF8);
            }
        }
        catch
        {
            // Never throw from the logger.
        }
    }
}
