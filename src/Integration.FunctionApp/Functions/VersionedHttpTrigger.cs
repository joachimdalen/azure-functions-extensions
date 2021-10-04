using System.Threading.Tasks;
using System.Web.Http;
using JoachimDalen.AzureFunctions.Extensions.Attributes;
using JoachimDalen.AzureFunctions.Extensions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Integration.FunctionApp.Functions
{
    public static class VersionedHttpTrigger
    {
        [FunctionName("VersionedHttpTrigger")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "version")]
            HttpRequest req, ILogger log, [
                RequestValue(Location = RequestValueLocation.Header | RequestValueLocation.Query, Name = "apiVersion",
                    Aliases = new[]
                    {
                        "x-api-version"
                    })
            ]
            string version
        )
        {
            if (string.IsNullOrEmpty(version))
            {
                return new BadRequestResult();
            }

            log.LogInformation("Triggered for version {Version}", version);
            return new OkObjectResult(version);
        }
    }
}