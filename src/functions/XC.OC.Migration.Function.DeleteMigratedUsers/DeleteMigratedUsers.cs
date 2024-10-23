using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using XC.OC.Migration.Core.Application.Abstractions;
using XC.OC.Migration.Core.Application.Models;
using XC.OC.Migration.Infrastructure.Persistence.Services;

namespace XC.OC.Migration.Function.DeleteMigratedUsers;

public class DeleteMigratedUsers(ILogger<DeleteMigratedUsers> logger, IUsersRepository usersRepository)
{
    [Function(nameof(DeleteMigratedUsers))]
    public async Task Run([QueueTrigger("%DeleteMigratedUsersQueueName%", Connection = "%queueConnectionString%")] QueueMessage message)
    {
        logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");
        var inputRequest = System.Text.Json.JsonSerializer.Deserialize<OrderCloudDeleteMigratedUsersRequest>(message.MessageText);
        if (inputRequest == null)
        {
            logger.LogError($"Invalid message: {message.MessageText}");
            return;
        }
        
        for (int iteration = 0; iteration < inputRequest.Iterations; iteration++)
        {
            
        }
    }
}