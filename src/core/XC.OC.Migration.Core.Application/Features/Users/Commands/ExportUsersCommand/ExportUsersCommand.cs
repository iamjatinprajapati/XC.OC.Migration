using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XC.OC.Migration.Core.Application.Abstractions.Infrastructure;

namespace XC.OC.Migration.Core.Application.Features.Users.Commands.ExportUsersCommand
{
    public class ExportUsersCommand(IMessageQueueService messageQueueService, ILogger<ExportUsersCommand> logger) : IExportUsersCommand
    {
        public async Task<bool> Execute(ExportUsersRequest request)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("In {commandName} - Execute", nameof(ExportUsersCommand));
            }

            var message = new ExportUsersQueueMessage(request);
            await messageQueueService.SendMessageAsync(message);
            return true;
        }
    }

    public class ExportUsersQueueMessage : IQueueMessage<ExportUsersRequest>
    {
        public string QueueName => "export-users";

        public ExportUsersRequest Message { get; set; }

        public ExportUsersQueueMessage(ExportUsersRequest request)
        {
            Message = request;
        }
    }
}
