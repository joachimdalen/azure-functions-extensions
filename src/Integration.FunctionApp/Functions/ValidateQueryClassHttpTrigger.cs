using System.Threading.Tasks;
using Integration.FunctionApp.Models;
using JoachimDalen.AzureFunctions.Extensions.Attributes;
using JoachimDalen.AzureFunctions.Extensions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Integration.FunctionApp.Functions
{
    public class ValidateQueryClassHttpTrigger
    {
        [FunctionName(nameof(ValidateQueryClassHttpTrigger))]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "test/query/class-validate")]
            HttpRequest req, ILogger log,
            [QueryParam(true)] QueryParamContainer<QueryParamValuesWithValidation> queryParamContainer)
        {
            if (!queryParamContainer.IsValid)
            {
                return new BadRequestObjectResult(queryParamContainer.ValidationResults);
            }

            return new OkObjectResult(queryParamContainer.Params);
        }
    }
}