using System.Diagnostics.CodeAnalysis;

namespace XC.OC.Migration.Users.Api.Models
{
    public class GetUsersListRequest
    {
        public string UserNamePrefix { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public int PageIndex { get; set; } = 0;

        public int PageSize { get; set; } = 300;

        public string ApplicationName { get; set; } = "sitecore";
    }
}
