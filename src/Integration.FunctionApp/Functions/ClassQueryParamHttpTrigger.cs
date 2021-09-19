using System.Threading.Tasks;
using Integration.FunctionApp.Models;
using JoachimDalen.AzureFunctions.Extensions.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Integration.FunctionApp.Functions
{
    public static class ClassQueryParamHttpTrigger
    {
        [FunctionName(nameof(ClassQueryParamHttpTrigger))]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "test/query/class")]
            HttpRequest req, ILogger log, [QueryParam] QueryParamValues queryParamContainer)
        {
            var data = new
            {
                queryParamContainer.Name,
                queryParamContainer.Age,
                queryParamContainer.Id
                
            };
            return new OkObjectResult(data);
        }
    }
}