using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using XC.OC.Migration.Infrastructure.Persistence;

var host = new HostBuilder()
    .AddServiceDefaults()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration((context, config) =>
    {
        config.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("local.settings.json", optional: true, 
            reloadOnChange: true)
            .AddJsonFile($"local.settings.{context.HostingEnvironment.EnvironmentName.ToLower()}.json", optional: true, reloadOnChange: true)
            //Always add environment variables at the last to make sure we read all available variables
            //on higher environments
            .AddEnvironmentVariables();
    })
    .ConfigureServices((context, services) =>
    {
        services.AddPersistenceServices(context.Configuration);
        //services.AddApplicationInsightsTelemetryWorkerService();
        //services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

host.Run();