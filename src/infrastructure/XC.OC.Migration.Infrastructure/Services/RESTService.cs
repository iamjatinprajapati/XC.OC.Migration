using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using XC.OC.Migration.Core.Application.Abstractions.Infrastructure;
using XC.OC.Migration.Core.Application.Enums;

namespace XC.OC.Migration.Infrastructure.Services
{
    internal class RestService(IFlurlClientCache clients, ILogger<RestService> logger) : IRESTService
    {
        private IFlurlClient GetClient(string baseUrl)
        {
            return clients.GetOrAdd(baseUrl, baseUrl, builder =>
            {
                builder.WithSettings(settings =>
                {
                    settings.JsonSerializer = new DefaultJsonSerializer(new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = false,
                        IgnoreReadOnlyProperties = true,
                        IgnoreReadOnlyFields = true,
                        PropertyNamingPolicy = null
                    });
                });
                builder.OnError(error => {
                    logger.LogError(error.Exception.Message, 
                        error.Exception.StackTrace);
                });
            });
        }

        public async Task DeleteAsync(string baseUrl, string endPoint, string accessToken = "", Dictionary<string, string>? headers =  null, RequestType requestType = RequestType.Json)
        {
            IFlurlClient flurlClient = GetClient(baseUrl);
            var request = flurlClient.Request(endPoint);
            if (headers != null)
            {
                request.WithHeaders(headers);
            }
            if(!string.IsNullOrEmpty(accessToken))
            {
                request.WithOAuthBearerToken(accessToken);
            }
            await request.DeleteAsync();
        }

        public async Task<T> GetAsync<T>(string baseUrl, string endPoint, Dictionary<string, string>? headers = null, 
            string accessToken = "",
            RequestType requestType = RequestType.Json) where T : class
        {
            IFlurlClient flurlClient = GetClient(baseUrl);
            var request = flurlClient.Request(endPoint);
            if(!string.IsNullOrEmpty(accessToken))
            {
                request.WithOAuthBearerToken(accessToken);
            }
            var response = await request.GetJsonAsync<T>();
            return response;
        }

        public Task<T> PostAsync<T>(string baseUrl, string endPoint, object? body = null, Dictionary<string, string>? headers = null, RequestType requestType = RequestType.Json) where T : class
        {
            IFlurlClient flurlClient = GetClient(baseUrl);
            if (headers != null)
            {
                flurlClient = flurlClient.WithHeaders(headers);
            }
            
            throw new NotImplementedException();
        }

        public async Task<T> PostUrlEncodedAsync<T>(string baseUrl, string endPoint, 
            object? body = null, Dictionary<string, string>? headers = null, 
            RequestType requestType = RequestType.Json) where T : class
        {
            using (logger.BeginScope(baseUrl, endPoint)) {
                IFlurlClient flurlClient = GetClient(baseUrl);
                if (headers != null)
                {
                    if (logger.IsEnabled(LogLevel.Debug))
                        logger.LogDebug("Adding headers to request");
                    flurlClient = flurlClient.WithHeaders(headers);
                }
                if (logger.IsEnabled(LogLevel.Debug))
                    logger.LogDebug("Sending request.");

                T response = await flurlClient.Request(endPoint)
                    .PostUrlEncodedAsync(body)
                    .ReceiveJson<T>();

                if (logger.IsEnabled(LogLevel.Debug))
                    logger.LogDebug("Response received.");

                return response;
            }            
        }
    }
}
