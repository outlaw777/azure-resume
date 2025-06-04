using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Company.Function
{
    public class CounterEntity
    {
        [JsonProperty("id")]
        public string Id { get; set; } = "1";

        [JsonProperty("count")]
        public int Count { get; set; }
    }

    public class GetResumeCounter
    {
        private readonly ILogger<GetResumeCounter> _logger;
        private readonly CosmosClient _cosmosClient;

        public GetResumeCounter(ILogger<GetResumeCounter> logger, CosmosClient cosmosClient)
        {
            _logger = logger;
            _cosmosClient = cosmosClient;
        }

        [Function("GetResumeCounter")]
        public CounterEntity Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] Microsoft.AspNetCore.Http.HttpRequest req,
            [CosmosDB(databaseName: "AzureResume", collectionName: "Counter", ConnectionStringSetting = "AzureResumeConnectionString", Id = "1", PartitionKey = "1")] CounterEntity counter)
        {
            _logger.LogInformation("Processing request for resume counter.");

            // Ensure counter exists
            if (counter == null)
            {
                _logger.LogError("Counter not found.");
                return null; // Returning null tells Azure Functions output binding that no change is needed
            }

            // Increment the counter
            counter.Count++;

            return counter; // Automatically updates in CosmosDB via output binding
        }

        private class CosmosDBAttribute : Attribute
        {
        }
    }
}



