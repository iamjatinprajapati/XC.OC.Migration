var builder = DistributedApplication.CreateBuilder(args);

//var postgres = builder.AddPostgres("postgres")
//                .WithImage("ankane/pgvector")
//                .WithImageTag("latest");

//var migrationLogsDb = postgres.AddDatabase("migrationlogsdb");
//var ordersdb = postgres.AddDatabase("ordersdb");

var apiService = builder.AddProject<Projects.XC_OC_Migration_ApiService>("apiservice");

builder.AddProject<Projects.XC_OC_Migration_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.AddProject<Projects.XC_OC_Migration_Orders_Api>("xc-oc-migration-orders-api");
        //.WithReference(ordersdb);

builder.AddProject<Projects.XC_OC_Migration_Users_Api>("xc-oc-migration-users-api");
        //.WithReference(ordersdb);

builder.Build().Run();
