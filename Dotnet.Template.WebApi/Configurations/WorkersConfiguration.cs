using Dotnet.Template.Workers;
using Quartz;

namespace Dotnet.Template.WebApi.Configurations
{
    /// <summary>
    /// Configura os jobs para integração de sistemas ou execução de tarefas
    /// </summary>
    public static class WorkersConfiguration
	{
		/// <summary>
		/// Use https://www.freeformatter.com/cron-expression-generator-quartz.html
		/// para calcular as expressões para os jobs agendados
		/// </summary>
		/// <param name="services"></param>
		/// <param name="configuration"></param>
		public static void AddWorkers(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddQuartz(q =>
			{
                AddFormularioWorker(q, configuration);
			});
			services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
		}

        /// <summary>
        /// Configura o job recorrente para tratar as intregrações
        /// </summary>
        /// <param name="servicesCollection"></param>
        /// <param name="configuration"></param>
        public static void AddFormularioWorker(this IServiceCollectionQuartzConfigurator servicesCollection, IConfiguration configuration)
		{
			var cronSchedule = configuration.GetValue<string>("AlertSchedule");
			if (string.IsNullOrWhiteSpace(cronSchedule)) return;

            /* ############################# Formulários ############################# */
            // Create a "key" for the job
            var formulariosIntegrationJob = new JobKey("UsersIntegrationJob");
			servicesCollection.AddJob<UsersIntegrationJob>(opts => opts.WithIdentity(formulariosIntegrationJob));
			servicesCollection.AddTrigger(opts => opts
				.ForJob(formulariosIntegrationJob)
				.WithIdentity($"{formulariosIntegrationJob.Name}-trigger")
				//.StartNow());
				.WithCronSchedule(cronSchedule));

		}
	}
}
