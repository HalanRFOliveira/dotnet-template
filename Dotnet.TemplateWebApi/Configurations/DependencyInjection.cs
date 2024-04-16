using Serilog;
using MySqlConnector;
using Dotnet.Template.Data;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Dotnet.Template.Infra.Resources;
using Microsoft.EntityFrameworkCore;
using Dotnet.Template.Domain.Globalization;
using Dotnet.Template.Infra.Mediator;

namespace Dotnet.Template.WebApi.Configurations
{
    public static class DependencyInjection
    {
        public static void AddDependencyInjectionConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            if (services == null) throw new ArgumentNullException(nameof(services));

            Log.Information("Injecting Dependencies");

            services.AddDbContext<MySqlContext>(options =>
            {
                //Configure the database you have (MSSQL, MySQL, etc) in the current version
                options.UseMySql(configuration.GetConnectionString("Template"), new MySqlServerVersion(new Version(5, 7, 38)));
            });

            services.AddHealthChecks()
                .AddAsyncCheck("api-portador-database", async () =>
                {
                    using var connection = new MySqlConnection(configuration.GetConnectionString("Template"));
                    try
                    {
                        await connection.OpenAsync();
                    }
                    catch (Exception)
                    {
                        return HealthCheckResult.Unhealthy();
                    }

                    await connection.CloseAsync();
                    await connection.DisposeAsync();
                    return HealthCheckResult.Healthy();
                });

            var serviceProvider = services.BuildServiceProvider();

            ResourceFactory.Factory = serviceProvider.GetService<IStringLocalizerFactory>();
            ResourceFactory.SetAssembly(typeof(GlobalizationConstants).Assembly.FullName);


            RegisterDomainDependencies(services);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // In case of have an activity log configuration
            //services.AddScoped<ActivityLogHelper>();

        }
        public static void RegisterDomainDependencies(IServiceCollection services)
        {
            //Register at least one Domain Class here
            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetUsersCommandHandler).Assembly));
            services.AddScoped<IMediatorHandler, MediatorHandler>();
        }
    }
}
