using System;
using Xunit;
using Q2Browser.Core.Models;
using Q2Browser.Core.Protocol;

namespace Q2Browser.Core.Tests;

public class SettingsTests
{
    [Fact]
    public void MasterServerPort_ValidPort_SetsValue()
    {
        var settings = new Settings();
        settings.MasterServerPort = 27900;
        Assert.Equal(27900, settings.MasterServerPort);
    }

    [Fact]
    public void MasterServerPort_MinimumPort_SetsValue()
    {
        var settings = new Settings();
        settings.MasterServerPort = 1;
        Assert.Equal(1, settings.MasterServerPort);
    }

    [Fact]
    public void MasterServerPort_MaximumPort_SetsValue()
    {
        var settings = new Settings();
        settings.MasterServerPort = 65535;
        Assert.Equal(65535, settings.MasterServerPort);
    }

    [Fact]
    public void MasterServerPort_Zero_ThrowsArgumentOutOfRangeException()
    {
        var settings = new Settings();
        Assert.Throws<ArgumentOutOfRangeException>(() => 
            settings.MasterServerPort = 0);
    }

    [Fact]
    public void MasterServerPort_TooLarge_ThrowsArgumentOutOfRangeException()
    {
        var settings = new Settings();
        Assert.Throws<ArgumentOutOfRangeException>(() => 
            settings.MasterServerPort = 65536);
    }

    [Fact]
    public void MasterServerPort_Negative_ThrowsArgumentOutOfRangeException()
    {
        var settings = new Settings();
        Assert.Throws<ArgumentOutOfRangeException>(() => 
            settings.MasterServerPort = -1);
    }

    [Fact]
    public void MasterServerAddress_Null_ThrowsArgumentNullException()
    {
        var settings = new Settings();
        Assert.Throws<ArgumentNullException>(() => 
            settings.MasterServerAddress = null!);
    }

    [Fact]
    public void HttpMasterServerUrl_ValidHttpUrl_SetsValue()
    {
        var settings = new Settings();
        settings.HttpMasterServerUrl = "http://example.com";
        Assert.Equal("http://example.com", settings.HttpMasterServerUrl);
    }

    [Fact]
    public void HttpMasterServerUrl_ValidHttpsUrl_SetsValue()
    {
        var settings = new Settings();
        settings.HttpMasterServerUrl = "https://example.com";
        Assert.Equal("https://example.com", settings.HttpMasterServerUrl);
    }

    [Fact]
    public void HttpMasterServerUrl_Null_SetsNull()
    {
        var settings = new Settings();
        settings.HttpMasterServerUrl = null;
        Assert.Null(settings.HttpMasterServerUrl);
    }

    [Fact]
    public void HttpMasterServerUrl_InvalidUrl_ThrowsArgumentException()
    {
        var settings = new Settings();
        Assert.Throws<ArgumentException>(() => 
            settings.HttpMasterServerUrl = "not-a-url");
    }

    [Fact]
    public void HttpMasterServerUrl_FtpUrl_ThrowsArgumentException()
    {
        var settings = new Settings();
        Assert.Throws<ArgumentException>(() => 
            settings.HttpMasterServerUrl = "ftp://example.com");
    }

    [Fact]
    public void MaxConcurrentProbes_ValidValue_SetsValue()
    {
        var settings = new Settings();
        settings.MaxConcurrentProbes = 75;
        Assert.Equal(75, settings.MaxConcurrentProbes);
    }

    [Fact]
    public void MaxConcurrentProbes_Minimum_SetsValue()
    {
        var settings = new Settings();
        settings.MaxConcurrentProbes = 1;
        Assert.Equal(1, settings.MaxConcurrentProbes);
    }

    [Fact]
    public void MaxConcurrentProbes_Maximum_SetsValue()
    {
        var settings = new Settings();
        settings.MaxConcurrentProbes = 200;
        Assert.Equal(200, settings.MaxConcurrentProbes);
    }

    [Fact]
    public void MaxConcurrentProbes_Zero_ThrowsArgumentOutOfRangeException()
    {
        var settings = new Settings();
        Assert.Throws<ArgumentOutOfRangeException>(() => 
            settings.MaxConcurrentProbes = 0);
    }

    [Fact]
    public void MaxConcurrentProbes_TooLarge_ThrowsArgumentOutOfRangeException()
    {
        var settings = new Settings();
        Assert.Throws<ArgumentOutOfRangeException>(() => 
            settings.MaxConcurrentProbes = 201);
    }

    [Fact]
    public void ProbeTimeoutMs_ValidValue_SetsValue()
    {
        var settings = new Settings();
        settings.ProbeTimeoutMs = 3000;
        Assert.Equal(3000, settings.ProbeTimeoutMs);
    }

    [Fact]
    public void ProbeTimeoutMs_Minimum_SetsValue()
    {
        var settings = new Settings();
        settings.ProbeTimeoutMs = 1;
        Assert.Equal(1, settings.ProbeTimeoutMs);
    }

    [Fact]
    public void ProbeTimeoutMs_Maximum_SetsValue()
    {
        var settings = new Settings();
        settings.ProbeTimeoutMs = 60000;
        Assert.Equal(60000, settings.ProbeTimeoutMs);
    }

    [Fact]
    public void ProbeTimeoutMs_Zero_ThrowsArgumentOutOfRangeException()
    {
        var settings = new Settings();
        Assert.Throws<ArgumentOutOfRangeException>(() => 
            settings.ProbeTimeoutMs = 0);
    }

    [Fact]
    public void ProbeTimeoutMs_TooLarge_ThrowsArgumentOutOfRangeException()
    {
        var settings = new Settings();
        Assert.Throws<ArgumentOutOfRangeException>(() => 
            settings.ProbeTimeoutMs = 60001);
    }

    [Fact]
    public void DefaultValues_AreCorrect()
    {
        var settings = new Settings();
        
        Assert.Equal("master.quake2.com", settings.MasterServerAddress);
        Assert.Equal(27900, settings.MasterServerPort);
        Assert.True(settings.UseHttpMasterServer);
        Assert.Equal("http://q2servers.com/?raw=2", settings.HttpMasterServerUrl);
        Assert.True(settings.EnableLanBroadcast);
        Assert.True(settings.RefreshOnStartup);
        Assert.True(settings.PortableMode);
        Assert.Equal("Warning", settings.LogLevel);
        Assert.Equal(75, settings.MaxConcurrentProbes);
        Assert.Equal(3000, settings.ProbeTimeoutMs);
        Assert.Equal(150, settings.UiUpdateIntervalMs);
        Assert.Empty(settings.Q2ExecutablePath);
    }
}

