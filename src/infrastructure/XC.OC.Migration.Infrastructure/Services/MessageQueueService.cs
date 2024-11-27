using Azure.Storage.Queues;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using XC.OC.Migration.Core.Application.Abstractions.Infrastructure;

namespace XC.OC.Migration.Infrastructure.Services
{
    public class MessageQueueService(QueueServiceClient client, ILogger<MessageQueueService> logger, IConfiguration configuration) : IMessageQueueService
    {
        public async Task SendMessageAsync<T>(IQueueMessage<T> message) where T : class
        {
            var serializedMessage = JsonConvert.SerializeObject(message.Message);
            if (logger.IsEnabled(LogLevel.Debug))
                logger.LogDebug("Sending message: {serializedMessage}", serializedMessage);

            QueueClient queueClient = client.GetQueueClient(message.QueueName);
            CancellationToken cancellationToken = new CancellationToken();
            await queueClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
            var base64Payload = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(serializedMessage));
            await queueClient.SendMessageAsync(base64Payload, cancellationToken);
        }
    }
}
