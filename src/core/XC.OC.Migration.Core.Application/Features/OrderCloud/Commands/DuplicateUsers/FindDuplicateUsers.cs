using Microsoft.Extensions.Logging;
using XC.OC.Migration.Core.Application.Abstractions.Infrastructure;
using XC.OC.Migration.Core.Application.Models;

namespace XC.OC.Migration.Core.Application.Features.OrderCloud.Commands.DuplicateUsers;

public class FindDuplicateUsersCommand(IMessageQueueService queueService, ILogger<FindDuplicateUsersCommand> logger) : IFindDuplicateUsersCommand
{
    public async Task Execute(FindDuplicateUsersRequest request)
    {
        if (logger.IsEnabled(LogLevel.Debug))
            logger.LogDebug("Finding duplicate users");

        var message = new FindDuplicateUsersMessage
        {
            
        };
        await queueService.SendMessageAsync(message);
    }
}