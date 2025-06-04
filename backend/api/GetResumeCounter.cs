using System.Configuration;
using System.Data.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Company.Function;
{

     public static class GetResumeCounter
    {


         [FunctionName("GetResumeCounter")]

         public static async Task<IActionResult> Run(
         [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
         [CosmosDB(databaseName: "AzureResume", collectionName: "Counter", ConnectionStringSettings = "AzureResumeConnectionString", Id = "1")] Counter counter,

         ILogger log)
    {

        log.LogInformation("C# HTTP trigger function processed a request.");

        string name = req.Query["name"];

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic data = JsonConvert.DeserializeObject(requestBody);
        name = name ?? data?.name;

        string responseMessage = string.IsNullOrEmpty(name)
        ? "This HTTP triggered fuction executed successfully. Pass a name in the query string or in the request body for a personalized response."
     : $"Hello, {name}. This HTTP triggered function executed successfully";

    }
  
  }        
             
}

    
