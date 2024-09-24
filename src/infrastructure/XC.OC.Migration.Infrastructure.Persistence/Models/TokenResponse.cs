namespace XC.OC.Migration.Infrastructure.Persistence.Models
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }

        public DateTime ExpiresUtc { get; set; }

        public string RefreshToken { get; set; }
    }
}