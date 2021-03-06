using System;
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
            var nameModel = JsonConvert.DeserializeObject<NameValueModel>(responseBody);

            Assert.AreEqual("hello", nameModel.Name);
        }
        
        [TestMethod]
        [StartFunctions(nameof(StringQueryParamHttpTrigger))]
        public async Task StringQueryParamHttpTrigger_QueryParamsWithArray_ReturnsValue()
        {
            var response = await Fixture.Client.GetAsync("/api/test/query/name?name=hello&values=1&values=2");

            var responseBody = await response.Content.ReadAsStringAsync();

            Assert.IsTrue(response.IsSuccessStatusCode);
            var nameModel = JsonConvert.DeserializeObject<NameValueModel>(responseBody);

            Assert.AreEqual(2, nameModel.Values.Length);
        }
        
        [TestMethod]
        [StartFunctions(nameof(StringQueryParamHttpTrigger))]
        public async Task StringQueryParamHttpTrigger_QueryParamsWithGuidArray_ReturnsValue()
        {
            var idOne = Guid.NewGuid();
            var idTwo = Guid.NewGuid();
            var response = await Fixture.Client.GetAsync($"/api/test/query/name?name=hello&ids={idOne.ToString()}&ids={idTwo.ToString()}");

            var responseBody = await response.Content.ReadAsStringAsync();

            Assert.IsTrue(response.IsSuccessStatusCode);
            var nameModel = JsonConvert.DeserializeObject<NameValueModel>(responseBody);

            Assert.AreEqual(2, nameModel.Ids.Length);
            Assert.AreEqual(idOne.ToString(), nameModel.Ids[0].ToString());
            Assert.AreEqual(idTwo.ToString(), nameModel.Ids[1].ToString());
        }
    }
}