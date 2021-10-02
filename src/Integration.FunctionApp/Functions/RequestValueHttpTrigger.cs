using System.Threading.Tasks;
using JoachimDalen.AzureFunctions.Extensions.Attributes;
using JoachimDalen.AzureFunctions.Extensions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Integration.FunctionApp.Functions
{
    public static class RequestValueHttpTrigger
    {
        [FunctionName(nameof(RequestValueHttpTrigger))]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "test/request-value")]
            HttpRequest req, ILogger log,
            [RequestValue(
                Location = RequestValueLocation.Header | RequestValueLocation.Query,
                Name = "x-api-key",
                Aliases = new[]
                {
                    "apiKey"
                }
            )]
            string value)
        {
            return new OkObjectResult(value);
        }
    }
}