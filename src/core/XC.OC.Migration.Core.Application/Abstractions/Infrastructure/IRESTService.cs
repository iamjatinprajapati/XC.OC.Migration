using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XC.OC.Migration.Core.Application.Enums;

namespace XC.OC.Migration.Core.Application.Abstractions.Infrastructure
{
    public interface IRESTService
    {
        Task<T> GetAsync<T>(string baseUrl, string endPoint, Dictionary<string, string>? headers = null, 
            string accessToken = "",
            RequestType requestType = RequestType.Json) where T : class;

        Task<T> PostAsync<T>(string baseUrl, string endPoint, object? body, Dictionary<string, string>? headers, RequestType requestType = RequestType.Json) where T : class;

        Task<T> PostUrlEncodedAsync<T>(string baseUrl,string endPoint, object? body = null, Dictionary<string, string>? headers = null, RequestType requestType = RequestType.Json) where T : class;

        Task DeleteAsync(string baseUrl, string endPoint, string accessToken = "", Dictionary<string, string>? headers = null, RequestType requestType = RequestType.Json);
    }
}
