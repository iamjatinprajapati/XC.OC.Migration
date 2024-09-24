using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace XC.OC.Migration.Function.DeleteMigratedOCUsers
{
    public class DeleteMigratedOCUsers
    {
        private readonly ILogger<DeleteMigratedOCUsers> _logger;

        public DeleteMigratedOCUsers(ILogger<DeleteMigratedOCUsers> logger)
        {
            _logger = logger;
        }

        [Function(nameof(DeleteMigratedOCUsers))]
        public void Run([QueueTrigger("%DeleteOCUsersQueue%", Connection = "%QueueConnectionString%")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");
        }
    }
}
