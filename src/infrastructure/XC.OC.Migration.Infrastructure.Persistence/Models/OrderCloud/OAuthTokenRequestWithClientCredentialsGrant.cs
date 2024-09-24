using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XC.OC.Migration.Infrastructure.Persistence.Models.OrderCloud
{
    public abstract class OAuthTokenRequest
    {
        public string grant_type { get; set; }

        public string client_id { get; set; }

        public string scope { get; set; }
    }

    public class OAuthTokenRequestWithClientCredentialsGrant : OAuthTokenRequest
    {
        public OAuthTokenRequestWithClientCredentialsGrant()
        {
            grant_type = "client_credentials";
        }

        public required string client_secret { get; set; }
    }

    public class OAuthTokenRequestWithPasswordGrant : OAuthTokenRequest
    {
        public OAuthTokenRequestWithPasswordGrant()
        {
            grant_type = "password";
        }

        public required string username { get; set; }

        public required string password { get; set; }
    }

    public class OAuthTokenResponse
    {
        public string access_token { get; set; }

        public string token_type { get; set; }

        public long expires_in { get; set; }

        public string refresh_token { get; set; }
    }
}
