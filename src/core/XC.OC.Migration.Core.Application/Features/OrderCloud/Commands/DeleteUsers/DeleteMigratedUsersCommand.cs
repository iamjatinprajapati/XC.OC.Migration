using Microsoft.Extensions.Logging;
using XC.OC.Migration.Core.Application.Abstractions;
using XC.OC.Migration.Core.Application.Abstractions.Infrastructure;
using XC.OC.Migration.Core.Application.Models;

namespace XC.OC.Migration.Core.Application.Features.OrderCloud.Commands.DeleteUsers
{
    public class DeleteMigratedUsersCommand(IMessageQueueService queueService, ILogger<DeleteMigratedUsersCommand> logger) : IDeleteMigratedUsersCommand
    {
        public async Task Execute(OrderCloudDeleteMigratedUsersRequest request)
        {
            if (logger.IsEnabled(LogLevel.Debug))
                logger.LogDebug("Deleting migrated users");

            var message = new DeleteOrderCloudMigratedUsersMessage
            {
                Iterations = request.Iterations
            };
            await queueService.SendMessageAsync(message);
        }
    }
}
