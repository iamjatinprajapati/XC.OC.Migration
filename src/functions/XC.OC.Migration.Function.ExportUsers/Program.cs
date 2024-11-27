using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using XC.OC.Migration.Infrastructure;
using XC.OC.Migration.Infrastructure.Persistence;

var builder = FunctionsApplication.CreateBuilder(args);
builder.AddAzureBlobClient("blobs");
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.ConfigureFunctionsWebApplication();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
