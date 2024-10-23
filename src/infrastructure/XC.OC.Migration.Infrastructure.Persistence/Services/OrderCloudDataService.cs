using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XC.OC.Migration.Core.Application.Abstractions.Infrastructure;
using XC.OC.Migration.Core.Domain.Model.Configurations;
using XC.OC.Migration.Core.Domain.Model.OrderCloud;
using XC.OC.Migration.Infrastructure.Persistence.Models;
using XC.OC.Migration.Infrastructure.Persistence.Models.OrderCloud;

namespace XC.OC.Migration.Infrastructure.Persistence.Services
{
    public interface IOrderCloudDataService
    {
        Task<TokenResponse> AuthenticateAsync(string clientId, string clientSecret, params string[] roles);

        Task<TokenResponse> AuthenticateAsync(string clientId, string userName, string password, params string[] roles);

        Task<TokenResponse> AuthenticateAsync(OAuthTokenRequest request);

        Task<TokenResponse> AuthenticateUsingClientCredentialsAsync();

        Task<PagedResults<User>> ListUsers(string accessToken, string sortBy = "", string filter = "");
        
        Task DeleteUser(string accessToken, string id);
    }

    public class OrderCloudDataService(IRESTService restService, ILogger<OrderCloudDataService> logger, IOptions<OrderCloudSettings> options) : 
        IOrderCloudDataService
    {
        private readonly OrderCloudSettings _orderCloudSettings = options.Value;

        public Task<TokenResponse> AuthenticateAsync(string clientId, string clientSecret, params string[] roles)
        {
            if (logger.IsEnabled(LogLevel.Debug))
                logger.LogDebug("Authenticating the user through client credentials grant");
            var request = new OAuthTokenRequestWithClientCredentialsGrant
            {
                client_id = clientId,
                client_secret = clientSecret,
                scope = string.Join(" ", roles)
            };
            return AuthenticateAsync(request);
        }

        public Task<TokenResponse> AuthenticateAsync(string clientId, string userName, string password, params string[] roles)
        {
            if (logger.IsEnabled(LogLevel.Debug))
                logger.LogDebug("Authenticating the user through password grant");
            var request = new OAuthTokenRequestWithPasswordGrant
            {
                client_id = clientId,
                username = userName,
                password = password,
                scope = string.Join(" ", roles)
            };
            return AuthenticateAsync(request);
        }

        public async Task<TokenResponse> AuthenticateAsync(OAuthTokenRequest request)
        {
            OAuthTokenResponse? response = await restService.PostUrlEncodedAsync<OAuthTokenResponse>(_orderCloudSettings.BaseUrl,
                "oauth/token", body: request);

            if(response == null || string.IsNullOrEmpty(response.access_token))
            {
                throw new UnauthorizedAccessException();
            }

            return new TokenResponse
            {
                AccessToken = response?.access_token!,
                // a bit arbitrary, but trim 30 minutes off the expiration to allow for latency
                ExpiresUtc = DateTime.UtcNow + TimeSpan.FromSeconds(response?.expires_in ?? 0) - TimeSpan.FromSeconds(30),
                RefreshToken = response?.refresh_token!
            };
        }

        public Task<TokenResponse> AuthenticateUsingClientCredentialsAsync()
        {
            return AuthenticateAsync(_orderCloudSettings.MiddlewareClientId, _orderCloudSettings.MiddlewareClientSecret);
        }

        public async Task<PagedResults<User>> ListUsers(string accessToken, string sortBy = "", string filter = "")
        {
            var endpoint = $"v1/buyers/{_orderCloudSettings.BuyerId}/users";
            bool hasQueryString = false;
            if(!string.IsNullOrEmpty(sortBy))
            {
                endpoint = $"{endpoint}?{sortBy}";
                hasQueryString = true ;
            }
            if(!string.IsNullOrEmpty(filter))
            {
                if(!hasQueryString)
                {
                    endpoint = $"{endpoint}?";
                    hasQueryString = true;
                }
                else
                {
                    endpoint = $"{endpoint}&{filter}";
                }
            }

            PagedResults<User> response = await restService.GetAsync<PagedResults<User>>(
                _orderCloudSettings.BaseUrl, endpoint, accessToken: accessToken);
            return response;
        }

        public async Task DeleteUser(string accessToken, string id)
        {
            var endpoint = $"v1/buyers/{_orderCloudSettings.BuyerId}/users/{id}";
            await restService.DeleteAsync(_orderCloudSettings.BaseUrl, endpoint, accessToken: accessToken);
        }
    }
}
