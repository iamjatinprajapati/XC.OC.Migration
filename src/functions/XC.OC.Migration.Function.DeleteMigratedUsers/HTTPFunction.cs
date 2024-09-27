using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace XC.OC.Migration.Function.DeleteMigratedUsers;

public class HTTPFunction
{
    private readonly ILogger<HTTPFunction> _logger;

    public HTTPFunction(ILogger<HTTPFunction> logger)
    {
        _logger = logger;
    }

    [Function("HTTPFunction")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
        
    }

}