using XC.OC.Migration.Core.Application.Models;

namespace XC.OC.Migration.Core.Application.Features.OrderCloud.Commands.DuplicateUsers;

public interface IFindDuplicateUsersCommand
{
    Task Execute(FindDuplicateUsersRequest request);
}