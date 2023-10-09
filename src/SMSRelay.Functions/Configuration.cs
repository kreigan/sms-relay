using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace SMSRelay.Functions;

public class Configuration
{
    private readonly ILogger _logger;

    public Configuration(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<Configuration>();
    }

    [Function("ConfigurationSetup")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "setup")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

        response.WriteString("Welcome to Azure Functions!");

        return response;
    }
}
