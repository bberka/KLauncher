using Serilog;
using Serilog.ConfigHelper;
using Serilog.ConfigHelper.Enricher;
using Serilog.Configuration;
using Serilog.Events;

namespace KLauncher.Core.Manager;

public static class LoggerConfigManager
{
    // private const string TemplateApi ="{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{IpAddress}] [{UriPath}] [{TraceId}] {Message:lj}{NewLine}{Exception}";
    // private const string TemplateClient = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

    private static readonly string LogPath = Path.Combine("Log", "KL_.log");

    public static void ApplyClientConfiguration() {
        Log.Logger = GetConfiguration(false).CreateLogger();
    }
    public static void ApplyApiConfiguration() {
        Log.Logger = GetConfiguration(true).CreateLogger();
    }

    
    private static LoggerConfiguration GetConfiguration(bool isApi) {
        var template = GetTemplate(isApi);
        var config = new LoggerConfiguration()
            .SetLogLevel(LogEventLevel.Information)
            .WriteTo.Console(outputTemplate: template)
            .WriteTo.File(
                LogPath,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 14,
                outputTemplate: template);
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


