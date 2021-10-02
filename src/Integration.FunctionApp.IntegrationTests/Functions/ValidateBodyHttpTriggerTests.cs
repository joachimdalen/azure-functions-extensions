using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using JoachimDalen.AzureFunctions.TestUtils.Attributes;
using Integration.FunctionApp.Functions;
using Integration.FunctionApp.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Integration.FunctionApp.IntegrationTests.Functions
{
    [TestClass]
    public class ValidateBodyHttpTriggerTests : BaseFunctionTest
    {
        [TestMethod]
        [StartFunctions(nameof(ValidateBodyHttpTrigger))]
        public async Task ValidateBodyHttpTrigger_InValidBody_ReturnsBadRequest()
        {
            var model = new BodyWithValidation
            {
                Username = "johndoe"
            };
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8,
                MediaTypeNames.Application.Json);
            var response = await Fixture.Client.PostAsync("/api/test/validate", content);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
        
        [TestMethod]
        [StartFunctions(nameof(ValidateBodyHttpTrigger))]
        public async Task ValidateBodyHttpTrigger_ValidBody_ReturnsOk()
        {
            var model = new BodyWithValidation
            {
                Username = "johndoe",
                Email = "john@doe.com"
            };
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8,
                MediaTypeNames.Application.Json);
            var response = await Fixture.Client.PostAsync("/api/test/validate", content);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}