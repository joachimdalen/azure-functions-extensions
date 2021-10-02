using System.Net.Http;
using System.Threading.Tasks;
using Integration.FunctionApp.Functions;
using JoachimDalen.AzureFunctions.TestUtils.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Integration.FunctionApp.IntegrationTests.Functions
{
    [TestClass]
    public class RequestValueHttpTriggerTests : BaseFunctionTest
    {
        [TestMethod]
        [StartFunctions(nameof(RequestValueHttpTrigger))]
        public async Task RequestValueHttpTrigger_HeaderValue_ReturnsValue()
        {
            var req = new HttpRequestMessage(HttpMethod.Get, "/api/test/request-value");
            req.Headers.Add("x-api-key", "myApiKey");
            
            var response = await Fixture.Client.SendAsync(req);
            var responseBody = await response.Content.ReadAsStringAsync();
            
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual("myApiKey", responseBody);
        }
    }
}