using Serilog;
using Serilog.Events;
using System.Globalization;
using Dotnet.Template.WebApi.Configurations;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Fatal)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Fatal)
    .WriteTo.File(@"logs\\log.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

//Add Authentication (Bearer Token, Microsoft, etc)
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);


var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

builder.Host.UseSerilog()
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.SetBasePath(Directory.GetCurrentDirectory());
        config.AddJsonFile("appsettings.json");
        config.AddJsonFile($"appsettings.{envName}.json", true);
    });

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Add your custom services
//builder.Services.AddEmailService(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLocalization(options => options.ResourcesPath = "Globalization");
builder.Services.AddSwaggerConfiguration();
builder.Services.AddDependencyInjectionConfiguration(builder.Configuration);

var app = builder.Build();

var ptBR = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentCulture = ptBR;
CultureInfo.DefaultThreadCurrentUICulture = ptBR;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseSerilogRequestLogging();
app.UseSwaggerSetup();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();
app.MapSwagger();

app.Lifetime.ApplicationStarted.Register(() =>
    Log.Information($"Application is running on {envName} | Use Swagger on: /swagger/index.html"));

app.Run();
