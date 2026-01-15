using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace Q2Connect.Wpf.Services;

public class DiagnosticLogger
{
    private static DiagnosticLogger? _instance;
    private readonly ObservableCollection<LogEntry> _logEntries = new();
    private readonly object _lockObject = new();
    private LogLevel _minimumLogLevel = LogLevel.Warning;

    public static DiagnosticLogger Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DiagnosticLogger();
            }
            return _instance;
        }
    }

    private DiagnosticLogger()
    {
    }

    public ObservableCollection<LogEntry> LogEntries => _logEntries;

    /// <summary>
    /// Sets the minimum log level from a string value (Debug, Info, Warning, Error).
    /// Defaults to Warning if the string is invalid.
    /// </summary>
    public void SetMinimumLogLevel(string logLevel)
    {
        _minimumLogLevel = logLevel?.ToLowerInvariant() switch
        {
            "debug" => LogLevel.Debug,
            "info" => LogLevel.Info,
            "warning" => LogLevel.Warning,
            "error" => LogLevel.Error,
            _ => LogLevel.Warning
        };
    }

    /// <summary>
    /// Checks if a log level should be logged based on the current minimum log level.
    /// </summary>
    private bool ShouldLog(LogLevel level)
    {
        return level >= _minimumLogLevel;
    }

    public void Log(LogLevel level, string message, string? details = null)
    {
        // Filter based on minimum log level
        if (!ShouldLog(level))
        {
            return;
        }

        var entry = new LogEntry
        {
            Timestamp = DateTime.Now,
            Level = level,
            Message = message,
            Details = details
        };

        try
        {
            if (Application.Current?.Dispatcher != null && !Application.Current.Dispatcher.HasShutdownStarted)
            {
                if (Application.Current.Dispatcher.CheckAccess())
                {
                    // Already on UI thread
                    lock (_lockObject)
                    {
                        _logEntries.Add(entry);
                        if (_logEntries.Count > 1000)
                        {
                            _logEntries.RemoveAt(0);
                        }
                    }
                }
                else
                {
                    // Need to invoke on UI thread
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        try
                        {
                            lock (_lockObject)
                            {
                                _logEntries.Add(entry);
                                if (_logEntries.Count > 1000)
                                {
                                    _logEntries.RemoveAt(0);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log to console as fallback
                            System.Diagnostics.Debug.WriteLine($"Logging error: {ex.Message}");
                        }
                    }), System.Windows.Threading.DispatcherPriority.Background);
                }
            }
            else
            {
                // Fallback if dispatcher not available
                lock (_lockObject)
                {
                    _logEntries.Add(entry);
                    if (_logEntries.Count > 1000)
                    {
                        _logEntries.RemoveAt(0);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Last resort: write to debug output
            System.Diagnostics.Debug.WriteLine($"Failed to log: {message} | Error: {ex.Message}");
        }
    }

    public void Clear()
    {
        try
        {
            if (Application.Current?.Dispatcher != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    lock (_lockObject)
                    {
                        _logEntries.Clear();
                    }
                });
            }
            else
            {
                lock (_lockObject)
                {
                    _logEntries.Clear();
                }
            }
        }
        catch
        {
            // Silently fail if clearing isn't available
        }
    }

    public void LogInfo(string message, string? details = null) => Log(LogLevel.Info, message, details);
    public void LogWarning(string message, string? details = null) => Log(LogLevel.Warning, message, details);
    public void LogError(string message, string? details = null) => Log(LogLevel.Error, message, details);
    public void LogDebug(string message, string? details = null) => Log(LogLevel.Debug, message, details);
}

public class LogEntry
{
    public DateTime Timestamp { get; set; }
    public LogLevel Level { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Details { get; set; }

    public string FormattedMessage => $"[{Timestamp:HH:mm:ss.fff}] [{Level}] {Message}";
}

public enum LogLevel
{
    Debug,
    Info,
    Warning,
    Error
}

