using System.Globalization;
using Serilog;
using Serilog.ConfigHelper;
using Serilog.ConfigHelper.Enricher;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace KLauncher.Core.Manager;

public static class LoggerConfigManager
{
    
    public static class Events
    {
        public static void OnProcessExit(object? sender, EventArgs e) {
            Log.CloseAndFlush();
        }
        
        public static void OnUnhandledException(object? sender, UnhandledExceptionEventArgs e) {
            Log.Fatal(e.ExceptionObject as Exception, "Unhandled exception occurred");
            Log.CloseAndFlush();
        }
        
    }
    // private const string TemplateApi ="{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{IpAddress}] [{UriPath}] [{TraceId}] {Message:lj}{NewLine}{Exception}";
    // private const string TemplateClient = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

    private static readonly string LogPath = Path.Combine("Log", "KL_.json");

    public static void ApplyClientConfiguration() {
        Log.Logger = GetConfiguration(false).CreateLogger();
    }
    public static void ApplyApiConfiguration() {
        Log.Logger = GetConfiguration(true).CreateLogger();
    }

    
    private static LoggerConfiguration GetConfiguration(bool isApi) {
        var template = GetTemplate(isApi);
        var test = new LoggerConfiguration()
            .SetLogLevel(LogEventLevel.Information,LogEventLevel.Debug)
            .WriteTo.Console(outputTemplate: template)
            .WriteTo.File(
                new CompactJsonFormatter(),
                LogPath,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 14);
        var config = new LoggerConfiguration()
            .SetLogLevel(LogEventLevel.Information,LogEventLevel.Debug)
            .WriteTo.Console(outputTemplate: template)
            .WriteTo.File(
                new CompactJsonFormatter(),
                LogPath,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 14);
        switch (isApi) {
            case true: // api
                config
                    .Enrich.WithIpAddressEnricher("IpAddress", "CF-Connecting-IP")
                    .Enrich.WithRequestPathEnricher("UriPath")
                    .Enrich.WithHttpRequestMethodEnricher("RequestMethod")
                    .Enrich.WithHttpRequestIdEnricher("RequestId");
                break;
            
            case false: // client
                config
                    .Enrich.WithThreadIdEnricher("ThreadId");
                break;
        }

        return config;
    }

    private static string GetTemplate(bool isApi) {
        switch (isApi) {
            case true: // api
                return new SerilogTemplateBuilder()
                    .AddTimeStamp()
                    .AddLevel()
                    .AddProperty("IpAddress",true)
                    .AddProperty("RequestMethod",true)
                    .AddProperty("UriPath",true)
                    .AddProperty("RequestId",true)
                    .AddMessage()
                    .AddException()
                    .Build();
            
            case false: // client
                return new SerilogTemplateBuilder()
                    .AddTimeStamp()
                    .AddLevel()
                    .AddProperty("ThreadId",true)
                    .AddMessage()
                    .AddException()
                    .Build();
        }
      
    }

   
}


