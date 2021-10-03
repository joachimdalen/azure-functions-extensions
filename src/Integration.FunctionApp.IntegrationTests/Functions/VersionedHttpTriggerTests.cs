using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Integration.FunctionApp.Functions;
using JoachimDalen.AzureFunctions.TestUtils.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Integration.FunctionApp.IntegrationTests.Functions
{
    [TestClass]
    public class VersionedHttpTriggerTests : BaseFunctionTest
    {
        [TestMethod]
        [StartFunctions(nameof(VersionedHttpTrigger))]
        public async Task VersionedHttpTrigger_ValueInQuery_ReturnsValue()
        {
            var response = await Fixture.Client.GetAsync("/api/version?apiVersion=v1");
            var responseBody = await response.Content.ReadAsStringAsync();

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual("v1", responseBody);
        }

        [TestMethod]
        [StartFunctions(nameof(VersionedHttpTrigger))]
        public async Task VersionedHttpTrigger_ValueInHeader_ReturnsValue()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/version");
            request.Headers.Add("x-api-version", "v2");
            var response = await Fixture.Client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual("v2", responseBody);
        }
        
        [TestMethod]
        [StartFunctions(nameof(VersionedHttpTrigger))]
        public async Task VersionedHttpTrigger_ValueInQuery_ReturnsError()
        {
            var response = await Fixture.Client.GetAsync("/api/version");
            
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}