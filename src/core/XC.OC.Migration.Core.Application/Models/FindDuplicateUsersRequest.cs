namespace XC.OC.Migration.Core.Application.Models;

public class FindDuplicateUsersRequest
{
    public string Folder { get; set; }

    public bool IsLocal { get; set; }

    public string Site { get; set; }
}