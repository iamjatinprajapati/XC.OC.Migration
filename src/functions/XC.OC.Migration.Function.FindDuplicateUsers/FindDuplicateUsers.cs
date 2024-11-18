using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace XC.OC.Migration.Function.FindDuplicateUsers;

public class FindDuplicateUsers
{
    private readonly ILogger<FindDuplicateUsers> _logger;

    public FindDuplicateUsers(ILogger<FindDuplicateUsers> logger)
    {
        _logger = logger;
    }

    [Function(nameof(FindDuplicateUsers))]
    public void Run([QueueTrigger("%FindDuplicateUsersQueue%", Connection = "%queueConnectionString%")] QueueMessage message)
    {
        _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");
    }
}