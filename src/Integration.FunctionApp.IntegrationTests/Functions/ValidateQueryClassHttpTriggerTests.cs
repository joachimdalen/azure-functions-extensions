using System.Net;
using System.Threading.Tasks;
using JoachimDalen.AzureFunctions.TestUtils.Attributes;
using Integration.FunctionApp.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Integration.FunctionApp.IntegrationTests.Functions
{
    [TestClass]
    public class ValidateQueryClassHttpTriggerTests : BaseFunctionTest
    {
        [TestMethod]
        [StartFunctions(nameof(ValidateQueryClassHttpTrigger))]
        public async Task ValidateQueryClassHttpTrigger_InValidBody_ReturnsBadRequest()
        {
            var response = await Fixture.Client.GetAsync("/api/test/query/class-validate?name=hello");
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
        
        [TestMethod]
        [StartFunctions(nameof(ValidateQueryClassHttpTrigger))]
        public async Task ValidateQueryClassHttpTrigger_ValidBody_ReturnsOk()
        {
            var response = await Fixture.Client.GetAsync("/api/test/query/class-validate?name=hello&age=10&info=hello22");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}