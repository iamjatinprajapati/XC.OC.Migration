using XC.OC.Migration.Core.Application.Models;

namespace XC.OC.Migration.Core.Application.Features.OrderCloud.Commands.DeleteUsers
{
    public interface IDeleteMigratedUsersCommand
    {
        Task Execute(OrderCloudDeleteMigratedUsersRequest request);
    }
}
