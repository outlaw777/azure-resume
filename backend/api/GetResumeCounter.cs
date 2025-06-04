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
        private readonly Container _container;

        public GetResumeCounter(ILogger<GetResumeCounter> logger, CosmosClient cosmosClient)
        {
            _logger = logger;
            _cosmosClient = cosmosClient;
            _container = _cosmosClient.GetContainer("AzureResume", "Counter");
        }

        [Function("GetResumeCounter")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] Microsoft.AspNetCore.Http.HttpRequest req)
        {
            _logger.LogInformation("Processing request for resume counter.");

            // Retrieve existing counter
            var response = await _container.ReadItemAsync<CounterEntity>("1", new PartitionKey("1"));
            var counter = response.Resource;

            if (counter == null)
            {
                return new NotFoundObjectResult("Counter not found.");
            }

            // Increment the counter
            counter.Count++;

            // Update the item in CosmosDB
            await _container.ReplaceItemAsync(counter, counter.Id, new PartitionKey(counter.Id));

            return new OkObjectResult(counter);
        }
    }
}



