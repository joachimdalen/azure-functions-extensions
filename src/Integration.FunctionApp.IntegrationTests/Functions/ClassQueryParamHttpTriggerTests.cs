using System.Net;
using System.Threading.Tasks;
using JoachimDalen.AzureFunctions.TestUtils.Attributes;
using Integration.FunctionApp.Functions;
using Integration.FunctionApp.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Integration.FunctionApp.IntegrationTests.Functions
{
    [TestClass]
    public class ClassQueryParamHttpTriggerTests : BaseFunctionTest
    {
        [TestMethod]
        [StartFunctions(nameof(ClassQueryParamHttpTrigger))]
        public async Task ClassQueryParamHttpTrigger_QueryParams_ReturnsValue()
        {
            var response =
                await Fixture.Client.GetAsync(
                    "/api/test/query/class?name=hello&age=10&id=41f56220-7f37-4f2e-aac9-b3515e4156cd");

            var responseBody = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(response.IsSuccessStatusCode);
            var nameModel = JsonConvert.DeserializeObject<QueryParamValues>(responseBody);

            Assert.AreEqual("hello", nameModel.Name);
            Assert.AreEqual(10, nameModel.Age);
            Assert.AreEqual("41f56220-7f37-4f2e-aac9-b3515e4156cd", nameModel.Id.ToString());
        }
    }
}