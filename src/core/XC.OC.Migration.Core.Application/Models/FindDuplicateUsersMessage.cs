using XC.OC.Migration.Core.Application.Abstractions.Infrastructure;

namespace XC.OC.Migration.Core.Application.Models;

public class FindDuplicateUsersMessage : IQueueMessage
{
    public string Folder { get; set; }

    public bool IsLocal { get; set; }

    public string Site { get; set; }
}