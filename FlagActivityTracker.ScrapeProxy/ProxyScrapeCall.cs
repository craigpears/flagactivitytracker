using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;

namespace FlagActivityTracker.ScrapeProxy
{
    public static class ProxyScrapeCall
    {
        [FunctionName("ProxyScrapeCall")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string pageUrl = req.Query["pageUrl"];

            log.LogInformation($"C# HTTP trigger function processed a request to {pageUrl}");

            using var httpClient = new HttpClient();
            var result = await httpClient.GetStringAsync(pageUrl);

            return new OkObjectResult(result);
        }
    }
}
