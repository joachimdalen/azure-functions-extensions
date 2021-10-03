using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using JoachimDalen.AzureFunctions.Extensions.Attributes;
using JoachimDalen.AzureFunctions.Extensions.Models;
using JoachimDalen.AzureFunctions.Extensions.ValueProviders;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JoachimDalen.AzureFunctions.Extensions.UnitTests.ValueProviders
{
    [TestClass]
    public class RequestValueProviderTests
    {
        [TestMethod]
        public async Task GetValueAsync_Header_ReturnsValue()
        {
            var request = TestUtils.GetRequest(HttpMethod.Get, "/test", new
            {
                Email = "john@doe.local"
            }, new Dictionary<string, string>
            {
                { "x-api-key", "myApiKey" }
            });

            var attribute = new RequestValueAttribute
            {
                Name = "x-api-key", Location = RequestValueLocation.Header
            };

            var valueProvider = new RequestValueProvider(request, attribute, GetInfo("myStringParam"), true, null);
            var val = await valueProvider.GetValueAsync();
            Assert.AreEqual("myApiKey", val);
        }

        [TestMethod]
        public async Task GetValueAsync_Query_ReturnsValue()
        {
            var request = TestUtils.GetRequest(HttpMethod.Get, "/test", new
            {
                Email = "john@doe.local"
            }, null, new Dictionary<string, StringValues>
            {
                { "apiKey", "myApiKey" }
            });

            var attribute = new RequestValueAttribute
            {
                Name = "apiKey", Location = RequestValueLocation.Query
            };

            var valueProvider = new RequestValueProvider(request, attribute, GetInfo("myStringParam"), true, null);
            var val = await valueProvider.GetValueAsync();
            Assert.AreEqual("myApiKey", val);
        }

        [TestMethod]
        public async Task GetValueAsync_None_ReturnsNull()
        {
            var request = TestUtils.GetRequest(HttpMethod.Get, "/test", new
            {
                Email = "john@doe.local"
            });

            var attribute = new RequestValueAttribute
            {
                Name = "apiKey", Location = RequestValueLocation.Query
            };

            var valueProvider = new RequestValueProvider(request, attribute, GetInfo("myStringParam"), true, null);
            var val = await valueProvider.GetValueAsync();
            Assert.AreEqual(null, val);
        }

        [TestMethod]
        public async Task GetValueAsync_AliasValueInQuery_ReturnsValue()
        {
            var request = TestUtils.GetRequest(HttpMethod.Get, "/test",
                new
                {
                    Email = "john@doe.local"
                },
                new Dictionary<string, string>
                {
                    { "x-api-keys", "headerApiKey" }
                },
                new Dictionary<string, StringValues>
                {
                    { "apiKey", "queryApiKey" }
                }
            );

            var attribute = new RequestValueAttribute
            {
                Name = "x-api-key",
                Aliases = new[] { "apiKey" },
                Location = RequestValueLocation.Query | RequestValueLocation.Header
            };

            var valueProvider = new RequestValueProvider(request, attribute, GetInfo("myStringParam"), true, null);
            var val = await valueProvider.GetValueAsync();
            Assert.AreEqual("queryApiKey", val);
        }

        [TestMethod]
        public async Task GetValueAsync_AliasValueInHeader_ReturnsValue()
        {
            var request = TestUtils.GetRequest(HttpMethod.Get, "/test",
                new
                {
                    Email = "john@doe.local"
                },
                new Dictionary<string, string>
                {
                    { "x-api-key", "headerApiKey" }
                },
                new Dictionary<string, StringValues>
                {
                    { "apisKey", "queryApiKey" }
                }
            );

            var attribute = new RequestValueAttribute
            {
                Name = "apiKey",
                Aliases = new[] { "x-api-key" },
                Location = RequestValueLocation.Query | RequestValueLocation.Header
            };

            var valueProvider = new RequestValueProvider(request, attribute, GetInfo("myStringParam"), true, null);
            var val = await valueProvider.GetValueAsync();
            Assert.AreEqual("headerApiKey", val);
        }

        private ParameterInfo GetInfo(string paramName)
        {
            var classType = typeof(TestDemoClass);
            var method = classType.GetMethod(nameof(TestDemoClass.MyTaskMethod));
            return method.GetParameters().FirstOrDefault(x => x.Name == paramName);
        }

        private class TestDemoClass
        {
            public Task MyTaskMethod(string myStringParam)
            {
                return Task.CompletedTask;
            }
        }
    }
}