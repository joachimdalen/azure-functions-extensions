using System.Threading.Tasks;
using Integration.FunctionApp.Models;
using JoachimDalen.AzureFunctions.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Integration.FunctionApp.Functions
{
    public static class ValidateBodyHttpTrigger
    {
        [FunctionName(nameof(ValidateBodyHttpTrigger))]
        public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "/test/validate")] HttpRequest req,
            ILogger log)
        {
            var reqData = await req.GetValidatedBody<BodyWithValidation>();
            if (!reqData.IsValid)
            {
                return new BadRequestObjectResult(reqData.ValidationResults);
            }

            return new OkObjectResult(reqData.Value);
        }
    }
}