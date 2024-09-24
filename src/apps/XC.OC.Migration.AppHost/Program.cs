using Aspire.Hosting;
using Microsoft.Extensions.Hosting;

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

builder.AddProject<Projects.XC_OC_Migration_Function_DeleteMigratedOCUsers>("xc-oc-migration-function-deletemigratedocusers")
    .WithReference(queues)
    .WithEnvironment("QueueConnectionString", "queueConnection")
    .WithEnvironment("DeleteOCUsersQueue", "delete-oc-users");

//builder.AddAzureFunction<Projects.XC_OC_Migration_Function_DeleteMigratedOCUsers>("xc-oc-migration-function-deletemigratedocusers", 50001, 50002)
//    .WithReference(queues);
//.WithReference(ordersdb);

builder.Build().Run();
