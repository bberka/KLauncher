using KLauncher.Core.Manager;
using KLauncher.ServerLib;
using KLauncher.ServerLib.Filters;
using KLauncher.ServerLib.Models;
using KLauncher.Shared.Interface;
using KLauncher.Shared.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(x => {
    x.Filters.Add<IpAddressFilter>();
    x.Filters.Add<ApiResponseFilter>();
    x.Filters.Add<UserAgentFilter>();
}).ConfigureApiBehaviorOptions(options => { options.InvalidModelStateResponseFactory = context => { return new HttpErrorResponseToResult(context).ToResult(); }; });
;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

builder.Services.AddResponseCompression();
builder.Services.AddScoped<IAuthenticator, AuthenticationManager>();
builder.Services.Configure<LauncherInformation>(builder.Configuration.GetSection(LauncherInformation.SectionName));


builder.Services.AddOptions<AuthServerConfiguration>()
    .BindConfiguration(AuthServerConfiguration.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseAuthentication();

app.UseResponseCompression();
app.UseStaticFiles();

app.MapControllers();

app.Run();