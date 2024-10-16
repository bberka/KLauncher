using KLauncher.Core.Manager;
using KLauncher.ServerLib;
using KLauncher.ServerLib.Filters;
using KLauncher.ServerLib.Models;
using KLauncher.Shared.Models;

#region LOGGER_CONFIG

LoggerConfigManager.ApplyApiConfiguration();
AppDomain.CurrentDomain.ProcessExit += LoggerConfigManager.Events.OnProcessExit;
AppDomain.CurrentDomain.UnhandledException += LoggerConfigManager.Events.OnUnhandledException;

#endregion

var builder = WebApplication.CreateBuilder(args);
// builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
//     {
//         config.SetBasePath(Directory.GetCurrentDirectory());
//         config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
//         config.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
//         config.AddEnvironmentVariables();
//     });

// Add services to the container.

#region DEFAULT_SERVICES

// builder.Services.AddControllers(mvcOptions => mvcOptions
//     .AddResultConvention(resultStatusMap => resultStatusMap
//         .AddDefaultMap()
//         .For(ResultStatus.Ok, HttpStatusCode.OK, resultStatusOptions => resultStatusOptions
//             .For("POST", HttpStatusCode.Created)
//             .For("DELETE", HttpStatusCode.NoContent))
//         .Remove(ResultStatus.Forbidden)
//         .Remove(ResultStatus.Unauthorized)
//     ));
// builder.Services.AddControllers(mvcOptions => mvcOptions
//     .AddResultConvention(resultStatusMap => resultStatusMap
//         .AddDefaultMap()
//         .For(ResultStatus.Error, HttpStatusCode.BadRequest, resultStatusOptions => resultStatusOptions
//             .With((ctrlr, result) => string.Join("\r\n", result.ValidationErrors)))
//     ));
builder.Services.AddControllers(x => {
         x.Filters.Add<IpAddressFilter>();
         x.Filters.Add<ApiResponseFilter>();
         x.Filters.Add<UserAgentFilter>();
       })
       .ConfigureApiBehaviorOptions(options => { options.InvalidModelStateResponseFactory = context => { return new HttpErrorResponseToResult(context).ToResult(); }; });
;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

#region DEPENDENCY_INJECTION

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();
// builder.Services.AddSerilog();
builder.Services.AddSingleton<GameFileManager>();
// builder.Services.AddScoped<IpAddressCheckMiddleware>();
// builder.Services.AddScoped<UserAgentCheckMiddleware>();
// builder.Services.AddScoped<ApiResponseToResultMiddleware>();

builder.Services.AddResponseCompression();

// builder.Services.Configure<ServerConfiguration>(builder.Configuration.GetSection(ServerConfiguration.SectionName));
builder.Services.Configure<LauncherInformation>(builder.Configuration.GetSection(LauncherInformation.SectionName));
builder.Services.Configure<RootDirectory>(x => { x.Path = builder.Configuration.GetValue<string>(DownloadServerConfiguration.SectionName + ":ClientFilesDirectoryPath"); });

builder.Services.AddOptions<DownloadServerConfiguration>()
       .BindConfiguration(DownloadServerConfiguration.SectionName)
       .ValidateDataAnnotations()
       .ValidateOnStart();

// // Explicitly register the settings object by delegating to the IOptions object
// builder.Services.AddSingleton(resolver => 
//     resolver.GetRequiredService<IOptions<SlackApiSettings>>().Value);

#endregion


var app = builder.Build();

#region DEFAULT_CONFIG

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.UseResponseCompression();
app.UseStaticFiles();
// app.UseMiddleware<IpAddressCheckMiddleware>();
// app.UseMiddleware<UserAgentCheckMiddleware>();
// app.UseMiddleware<ApiResponseToResultMiddleware>();


app.MapControllers();

#endregion


// app.UseSerilogRequestLogging();


app.Run();