namespace XC.OC.Migration.Core.Application.Features.Users.Commands.ExportUsersCommand
{
    public record ExportUsersRequest
    {
        public string UserNamePrefix { get; init; }

        public string StartDate { get; init; }

        public string EndDate { get; init; }

        public int PageSize { get; init; }

        public List<string> Emails { get; init; }

        public string ApplicationName { get; init; }

        public string FileNamePrefix { get; init; }

        public ExportUsersRequest()
        {
            Emails = new List<string>();
        }
    }
}
