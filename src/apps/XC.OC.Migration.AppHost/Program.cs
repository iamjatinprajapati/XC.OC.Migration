using Aspire.Hosting;
using Microsoft.Extensions.Hosting;
using XC.OC.Migration.AppHost;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

//var postgres = builder.AddPostgres("postgres")
//                .WithImage("ankane/pgvector")
//                .WithImageTag("latest");

//var migrationLogsDb = postgres.AddDatabase("migrationlogsdb");
//var ordersdb = postgres.AddDatabase("ordersdb");

//Azure storage service
IResourceBuilder<Aspire.Hosting.Azure.AzureStorageResource> storage = builder.AddAzureStorage("storage");

if(builder.Environment.IsDevelopment())
{
    storage.RunAsEmulator(configuration =>
    {
        configuration.WithBlobPort(60001).WithQueuePort(60002).WithTablePort(60003);
    });
}

IResourceBuilder<Aspire.Hosting.Azure.AzureQueueStorageResource> queues = storage.AddQueues("queueConnection");

IResourceBuilder<ProjectResource> apiService = builder.AddProject<Projects.XC_OC_Migration_ApiService>("apiservice");

builder.AddProject<Projects.XC_OC_Migration_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.AddProject<Projects.XC_OC_Migration_Orders_Api>("xc-oc-migration-orders-api");
        //.WithReference(ordersdb);

builder.AddProject<Projects.XC_OC_Migration_Users_Api>("xc-oc-migration-users-api")
        .WithReference(queues);
//.WithReference(ordersdb);

builder.AddAzureFunction<Projects.XC_OC_Migration_Function_DeleteMigratedUsers>(
        "xc-oc-migration-function-delete-migrated-oc-users")
    .WithReference(queues)
    .WithEnvironment("queueConnectionString", "queueConnection");

builder.AddAzureFunction<Projects.XC_OC_Migration_Function_FindDuplicateUsers>(
    "xc-oc-migration-function-find-duplicate-users")
    .WithReference(queues)
    .WithEnvironment("queueConnectionString", "queueConnection");;

builder.Build().Run();
