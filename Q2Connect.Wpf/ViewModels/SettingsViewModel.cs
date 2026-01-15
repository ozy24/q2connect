using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using Q2Connect.Core.Models;
using Q2Connect.Core.Protocol;
using Q2Connect.Core.Services;

namespace Q2Connect.Wpf.ViewModels;

public class SettingsViewModel : INotifyPropertyChanged
{
    private readonly FavoritesService _favoritesService;
    private Settings _settings;

    public SettingsViewModel()
    {
        // Use DiagnosticLogger as adapter for FavoritesService
        var logger = new Services.CoreLoggerAdapter();
        _favoritesService = new FavoritesService(logger);
        _settings = new Settings();
        
        SaveCommand = new RelayCommand(async _ => await SaveSettingsAsync());
        BrowseQ2ExecutableCommand = new RelayCommand(_ => BrowseQ2Executable());
        CancelCommand = new RelayCommand(_ => { });
        
        _ = LoadSettingsAsync();
    }

    private async Task LoadSettingsAsync()
    {
        _settings = await _favoritesService.LoadSettingsAsync();
        OnPropertyChanged(nameof(MasterServerAddress));
        OnPropertyChanged(nameof(MasterServerPort));
        OnPropertyChanged(nameof(UseHttpMasterServer));
        OnPropertyChanged(nameof(HttpMasterServerUrl));
        OnPropertyChanged(nameof(EnableLanBroadcast));
        OnPropertyChanged(nameof(RefreshOnStartup));
        OnPropertyChanged(nameof(PortableMode));
        OnPropertyChanged(nameof(Q2ExecutablePath));
        OnPropertyChanged(nameof(MaxConcurrentProbes));
        OnPropertyChanged(nameof(ProbeTimeoutMs));
        OnPropertyChanged(nameof(LogLevel));
    }

    public string MasterServerAddress
    {
        get => _settings.MasterServerAddress;
        set
        {
            if (_settings.MasterServerAddress != value)
            {
                _settings.MasterServerAddress = value;
                OnPropertyChanged();
            }
        }
    }

    public string MasterServerPort
    {
        get => _settings.MasterServerPort.ToString();
        set
        {
            if (int.TryParse(value, out var port) && port > 0 && port < 65536)
            {
                if (_settings.MasterServerPort != port)
                {
                    _settings.MasterServerPort = port;
                    OnPropertyChanged();
                }
            }
        }
    }

    public bool UseHttpMasterServer
    {
        get => _settings.UseHttpMasterServer;
        set
        {
            if (_settings.UseHttpMasterServer != value)
            {
                _settings.UseHttpMasterServer = value;
                OnPropertyChanged();
            }
        }
    }

    public string? HttpMasterServerUrl
    {
        get => _settings.HttpMasterServerUrl;
        set
        {
            if (_settings.HttpMasterServerUrl != value)
            {
                // Validate URL if not null/empty - allow setting but validate on save
                // This allows user to type the URL and see validation feedback when saving
                try
                {
                    _settings.HttpMasterServerUrl = value;
                    OnPropertyChanged();
                }
                catch (ArgumentException ex)
                {
                    // Invalid URL format - show error but don't prevent user from editing
                    // The validation error will be shown when user tries to save
                    System.Windows.MessageBox.Show(
                        $"Invalid URL format: {ex.Message}\n\nPlease enter a valid HTTP or HTTPS URL.",
                        "Invalid URL",
                        System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Warning);
                }
            }
        }
    }

    public bool EnableLanBroadcast
    {
        get => _settings.EnableLanBroadcast;
        set
        {
            if (_settings.EnableLanBroadcast != value)
            {
                _settings.EnableLanBroadcast = value;
                OnPropertyChanged();
            }
        }
    }

    public bool RefreshOnStartup
    {
        get => _settings.RefreshOnStartup;
        set
        {
            if (_settings.RefreshOnStartup != value)
            {
                _settings.RefreshOnStartup = value;
                OnPropertyChanged();
            }
        }
    }

    public bool PortableMode
    {
        get => _settings.PortableMode;
        set
        {
            if (_settings.PortableMode != value)
            {
                _settings.PortableMode = value;
                OnPropertyChanged();
            }
        }
    }

    public string Q2ExecutablePath
    {
        get => _settings.Q2ExecutablePath;
        set
        {
            if (_settings.Q2ExecutablePath != value)
            {
                _settings.Q2ExecutablePath = value;
                OnPropertyChanged();
            }
        }
    }

    public string MaxConcurrentProbes
    {
        get => _settings.MaxConcurrentProbes.ToString();
        set
        {
            if (int.TryParse(value, out var probes) && probes > 0 && probes <= 200)
            {
                if (_settings.MaxConcurrentProbes != probes)
                {
                    _settings.MaxConcurrentProbes = probes;
                    OnPropertyChanged();
                }
            }
        }
    }

    public string ProbeTimeoutMs
    {
        get => _settings.ProbeTimeoutMs.ToString();
        set
        {
            if (int.TryParse(value, out var timeout) && timeout > 0 && timeout <= 30000)
            {
                if (_settings.ProbeTimeoutMs != timeout)
                {
                    _settings.ProbeTimeoutMs = timeout;
                    OnPropertyChanged();
                }
            }
        }
    }

    public string LogLevel
    {
        get => _settings.LogLevel;
        set
        {
            // Validate log level
            var validLevels = new[] { "Debug", "Info", "Warning", "Error" };
            if (validLevels.Contains(value, StringComparer.OrdinalIgnoreCase))
            {
                if (_settings.LogLevel != value)
                {
                    _settings.LogLevel = value;
                    OnPropertyChanged();
                }
            }
        }
    }

    public ICommand SaveCommand { get; }
    public ICommand BrowseQ2ExecutableCommand { get; }
    public ICommand CancelCommand { get; }

    private void BrowseQ2Executable()
    {
        var dialog = new OpenFileDialog
        {
            Filter = "Executable files (*.exe)|*.exe|All files (*.*)|*.*",
            Title = "Select Quake 2 Executable"
        };

        if (dialog.ShowDialog() == true)
        {
            Q2ExecutablePath = dialog.FileName;
        }
    }

    private async Task SaveSettingsAsync()
    {
        try
        {
            await _favoritesService.SaveSettingsAsync(_settings);
            
            // Update the DiagnosticLogger with the new log level
            Services.DiagnosticLogger.Instance.SetMinimumLogLevel(_settings.LogLevel);
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(
                $"Failed to save settings:\n\n{ex.Message}",
                "Error",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Error);
            throw; // Re-throw to prevent dialog from closing
        }
    }

    public Settings GetSettings() => _settings;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

