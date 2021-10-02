using System.Threading.Tasks;
using JoachimDalen.AzureFunctions.TestUtils.Attributes;
using Integration.FunctionApp.Functions;
using Integration.FunctionApp.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Integration.FunctionApp.IntegrationTests.Functions
{
    [TestClass]
    public class StringQueryParamHttpTriggerTests : BaseFunctionTest
    {
        [TestMethod]
        [StartFunctions(nameof(StringQueryParamHttpTrigger))]
        public async Task StringQueryParamHttpTrigger_QueryParams_ReturnsValue()
        {
            var response = await Fixture.Client.GetAsync("/api/test/query/name?name=hello");

            var responseBody = await response.Content.ReadAsStringAsync();

            Assert.IsTrue(response.IsSuccessStatusCode);
            var nameModel = JsonConvert.DeserializeObject<NameModel>(responseBody);

            Assert.AreEqual("hello", nameModel.Name);
        }
    }
}