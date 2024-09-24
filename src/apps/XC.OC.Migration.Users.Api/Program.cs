using Microsoft.AspNetCore.Mvc;
using XC.OC.Migration.Core.Application.Features.Users.Queries.GetUsersList;
using XC.OC.Migration.Users.Api.Models;
using XC.OC.Migration.Core.Application;
using XC.OC.Migration.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

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

usersApis.MapPost("/", async ([FromBody]GetUsersListRequest request,
    IGetUsersListQuery query) => await query.Execute(request.UserNamePrefix, request.StartDate, request.EndDate, request.PageIndex,
    request.PageSize, request.ApplicationName))
    .WithName("GetXCUsers")
    .WithOpenApi();

app.Run();
