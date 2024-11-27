using Microsoft.AspNetCore.Mvc;
using XC.OC.Migration.Core.Application;
using XC.OC.Migration.Infrastructure.Persistence;
using XC.OC.Migration.Core.Application.Features.Users.Commands.ExportUsersCommand;

var builder = WebApplication.CreateBuilder(args);

builder.AddAzureQueueClient("queueConnection");
builder.AddAzureBlobClient("blobs");

builder.AddServiceDefaults();

builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var usersApis = app.MapGroup("users");

usersApis.MapPost("/export", async ([FromBody] ExportUsersRequest request, IExportUsersCommand command) =>
{
    if (request != null)
    {
        var result = await command.Execute(request);
    }
    
}).WithName("ExportUsers").WithOpenApi();

//usersApis.MapPost("/", async ([FromBody]GetUsersListRequest request,
//    IGetUsersListQuery query) => await query.Execute(request.UserNamePrefix, request.StartDate, request.EndDate, 
//        request.PageIndex, request.PageSize, request.ApplicationName))
//            .WithName("GetXCUsers")
//            .WithOpenApi();


//var ocUsers = app.MapGroup("oc-users");
//ocUsers.MapPost("/delete", async ([FromBody]OrderCloudDeleteMigratedUsersRequest message, 
//    IDeleteMigratedUsersCommand request) =>
//{
//    await request.Execute(message);
//    return Results.Accepted();
//});

//ocUsers.MapPost("/find-duplicate-users",
//    async ([FromBody] FindDuplicateUsersRequest message, IFindDuplicateUsersCommand request) =>
//    {
//        await request.Execute(message);
//        return Results.Accepted();
//    });

app.Run();
