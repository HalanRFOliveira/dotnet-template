using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

namespace Dotnet.Template.WebApi.Configurations
{
    /// <summary>
    /// Classe de configuração do swagger
    /// </summary>
    public static class SwaggerConfiguration
    {
        /// <summary>
        /// Adiciona a configuração do swagger
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            Log.Information("Configuring Swagger");

            ArgumentNullException.ThrowIfNull(services);

            var apiFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Assembly.GetExecutingAssembly().GetName().Name + ".xml");

            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(type => type.ToString());

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Dotnet Template Web API",
                    Description = "API de consumo Dotnet Template",
                    Version = "v1",
                    Contact = new OpenApiContact()
                    {
                        Name = "Halan Oliveira",
                        Url = new Uri("https://github.com/HalanRFOliveira"),
                    }
                });

                c.IncludeXmlComments(apiFilePath);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Adicione o token de autenticação no campo abaixo \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });
        }

        /// <summary>
        /// Configura o swagger
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwaggerSetup(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app);

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "v1");
                c.RoutePrefix = "api";
            });
        }
    }
}
