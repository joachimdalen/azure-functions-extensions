using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using AzureFunctions.TestUtils.Asserts;
using AzureFunctions.TestUtils.Attributes;
using Integration.FunctionApp.Functions;
using Integration.FunctionApp.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Integration.FunctionApp.IntegrationTests.Functions
{
    [TestClass]
    public class MultipartHttpTriggerTests : BaseFunctionTest
    {
        [TestMethod]
        [StartFunctions(nameof(MultipartHttpTrigger))]
        [UseBlobContainers("multipart-files")]
        public async Task MultipartHttpTrigger_ContentWithFiles_ReturnsValue()
        {
            var multipart = new MultipartFormDataContent();
            var jsonData = new MultipartRequestBodyData
            {
                Email = "john@doe.local",
                Username = "johndoe"
            };
            multipart.Add(new StringContent(JsonConvert.SerializeObject(jsonData), Encoding.UTF8,
                MediaTypeNames.Application.Json));

            var bytes = Encoding.UTF8.GetBytes("Hello");
            var bytes2 = Encoding.UTF8.GetBytes("Goodbye");
            multipart.Add(new ByteArrayContent(bytes, 0, bytes.Length), "file", "greeting.txt");
            multipart.Add(new ByteArrayContent(bytes2, 0, bytes2.Length), "file", "goodbye.txt");


            var request = new HttpRequestMessage(HttpMethod.Post, "api/test/multipart")
            {
                Content = multipart
            };

            var response = await Fixture.Client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(response.IsSuccessStatusCode);
            var model = JsonConvert.DeserializeObject<MultipartRequestBodyData>(responseBody);

            Assert.AreEqual(jsonData.Email, model.Email);
            Assert.AreEqual(jsonData.Username, model.Username);
            Assert.That.BlobExists("multipart-files", "greeting.txt");
        }
    }
}