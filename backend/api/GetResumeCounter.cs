using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
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

        public int Count { get; private set; }

        [Function("GetResumeCounter")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(databaseName: "AzureResume", collectionName: "Counter", ConnectionStringSetting = "AzureResumeConnectionString", Id = "1", PartitionKey = "1")] GetResumeCounter counter,
            [CosmosDB(databaseName: "AzureResume", collectionName: "Counter", ConnectionStringSetting = "AzureResumeConnectionString", Id = "1", PartitionKey = "1")] out GetResumeCounter updatedGetResumeCounter,
            ILogger log)
        {
            _logger.LogInformation("Processing request for resume counter.");

            // Ensure the counter exists
            if (counter == null)
            {
                return new NotFoundObjectResult("Counter not found.");
            }

            // Increment the counter
            updatedGetResumeCounter = counter;
            updatedGetResumeCounter.Count++;

            var jsonToReturn = JsonConvert.SerializeObject(updatedGetResumeCounter);

            return new OkObjectResult(jsonToReturn);
        }
    }
}

