namespace XC.OC.Migration.ApiService.Models
{
    public class UsersCountRequest
    {
        public string UserNamePrefix { get; set; }

        public string ApplicationName { get; set; } = "sitecore";

        public int PageIndex { get; set; } = 0;

        public int PageSize { get; set; } = 300;

        public string StartDate { get; set; }

        public string EndDate { get; set; } 
    }
}
