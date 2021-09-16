using System.Threading.Tasks;
using JoachimDalen.AzureFunctions.Extensions.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Integration.FunctionApp.Functions
{
    public static class StringQueryParamHttpTrigger
    {
        [FunctionName(nameof(StringQueryParamHttpTrigger))]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "test/query/name")]
            HttpRequest req, ILogger log, [QueryParam("name")] string name)
        {
            var data = new
            {
                Name = name
            };
            return new OkObjectResult(data);
        }
    }
}