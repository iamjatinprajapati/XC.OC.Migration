using Azure.Storage.Queues;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XC.OC.Migration.Core.Application.Abstractions.Infrastructure;

namespace XC.OC.Migration.Infrastructure.Services
{
    public class MessageQueueService(QueueServiceClient client, ILogger<MessageQueueService> logger) : IMessageQueueService
    {
        public async Task SendMessageAsync(IQueueMessage message)
        {
            var serializedMessage = JsonConvert.SerializeObject(message);
            if(logger.IsEnabled(LogLevel.Debug))
                logger.LogDebug("Sending message: {serializedMessage}", serializedMessage);

            QueueClient queueClient = client.GetQueueClient("delete-oc-users");
            CancellationToken cancellationToken = new CancellationToken();
            await queueClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
            var base64Payload = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(serializedMessage));
            await queueClient.SendMessageAsync(base64Payload);
        }
    }
}
