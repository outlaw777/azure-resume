using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Company.Function
{
    public class GetResumeCounter
    {
        private readonly ILogger<GetResumeCounter> _logger;

        public GetResumeCounter(ILogger<GetResumeCounter> logger)
        {
            _logger = logger;
        }

        [Function("GetResumeCounter")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
            [CosmosDBInput(databaseName: "AzureResume", containerName: "Counter", Connection = "AzureResumeConnectionString", Id = "1")] GetResumeCounter counter)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            string name = req.Query["name"];
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            using var reader = new StreamReader(req.Body);
            string requestBody = await reader.ReadToEndAsync();
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            dynamic data = JsonConvert.DeserializeObject(requestBody);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            name = name ?? data?.name;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This function executed successfully. Provide a name for a personalized response."
                : $"Hello, {name}. This function executed successfully.";

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");
            await response.WriteStringAsync(responseMessage);

            return response;
        }
    }
}

    



