using Microsoft.AspNetCore.Http.HttpResults;
using XC.OC.Migration.ApiService.Models;
using XC.OC.Migration.Core.Application.Abstractions;
using XC.OC.Migration.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddPersistenceServices(builder.Configuration);

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapGet("/order-cloud/users", async (IUsersRepository usersRepository) => 
await usersRepository.GetOrderCloudUsers())
    .WithName("Get order cloud users");

app.MapPost("/order-cloud/users/delete", () =>
{
    return Results.Ok();
}).WithName("Delete order cloud user");

app.MapPost("/xc/users-count", async (UsersCountRequest request, IUsersRepository usersRepository) => 
await usersRepository.GetUsersCount(request.UserNamePrefix, request.StartDate, request.EndDate, request.PageIndex, request.PageSize,
request.ApplicationName))
    .WithName("Get XC Users count");

app.MapPost("/xc/users", async (UsersCountRequest request, IUsersRepository usersRepository) =>
await usersRepository.GetUsersCount(request.UserNamePrefix, request.StartDate, request.EndDate, request.PageIndex, request.PageSize,
request.ApplicationName))
    .WithName("Get XC Users");

app.MapDefaultEndpoints();

app.Run();