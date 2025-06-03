using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Function;

public class GetResumeCounter2
{
    private readonly ILogger<GetResumeCounter2> _logger;

    public GetResumeCounter2(ILogger<GetResumeCounter2> logger)
    {
        _logger = logger;
    }

    [Function("GetResumeCounter2")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}