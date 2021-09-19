using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using Integration.FunctionApp.Models;
using JoachimDalen.AzureFunctions.Extensions.Attributes;
using JoachimDalen.AzureFunctions.Extensions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Integration.FunctionApp.Functions
{
    public class MultipartHttpTrigger
    {
        [FunctionName(nameof(MultipartHttpTrigger))]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "test/multipart")]
            HttpRequest req, ILogger log,
            [MultipartRequest(ValidateData = true)]
            MultipartRequestData<MultipartRequestBodyData> multipartRequest,
            [Blob("multipart-files", FileAccess.Write)]
            CloudBlobContainer blobContainer)
        {
            foreach (var requestFile in multipartRequest.Files)
            {
                var blob = blobContainer.GetBlockBlobReference(requestFile.FileName);
                blob.Properties.ContentType = MediaTypeNames.Application.Octet;
                await blob.UploadFromByteArrayAsync(requestFile.Content, 0, requestFile.Content.Length);
            }

            var data = new
            {
                Username = multipartRequest.Data.Username,
                Email = multipartRequest.Data.Email
            };
            return new OkObjectResult(data);
        }
    }
}