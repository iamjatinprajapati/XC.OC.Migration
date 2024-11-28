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
        //configuration.WithBlobPort(60001).WithQueuePort(60002).WithTablePort(60003);
        configuration.WithArgs("azurite", "-l", "/data", "--blobHost", "0.0.0.0", "--queueHost", "0.0.0.0", "--tableHost", "0.0.0.0", "--skipApiVersionCheck");
    });
    //.WithAnnotation(new ContainerImageAnnotation
    //{
    //    Registry = "mcr.microsoft.com",
    //    Image = "azure-storage/azurite",
    //    Tag = "3.30.0"
    //}).WithParameter("skipApiVersionCheck");
}

IResourceBuilder<Aspire.Hosting.Azure.AzureQueueStorageResource> queues = storage.AddQueues("queueConnection");
var blobs = storage.AddBlobs("blobs");

builder.AddProject<Projects.XC_OC_Migration_Web>("webfrontend")
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.XC_OC_Migration_Orders_Api>("xc-oc-migration-orders-api");
        //.WithReference(ordersdb);

builder.AddProject<Projects.XC_OC_Migration_Users_Api>("xc-oc-migration-users-api")
        .WithReference(queues)
        .WithReference(blobs);
//.WithReference(ordersdb);

builder.AddProject<Projects.XC_OC_Migration_Function_ExportUsers>("xc-oc-migration-function-exportusers")
    .WithReference(queues)
    .WithEnvironment("queueConnectionString", "queueConnection")
    .WithReference(blobs)
    .WithEnvironment("AzureStorage:ContainerName", "xc-users");

builder.Build().Run();
