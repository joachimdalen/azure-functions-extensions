using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using JoachimDalen.AzureFunctions.Extensions.Attributes;
using JoachimDalen.AzureFunctions.Extensions.Models;
using JoachimDalen.AzureFunctions.Extensions.ValueProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace JoachimDalen.AzureFunctions.Extensions.UnitTests.ValueProviders
{
    [TestClass]
    public class MultipartRequestValueProviderTests
    {
        [TestMethod]
        public async Task GetValueAsync_NoFiles_ReturnsData()
        {
            var request = GetRequest(new MultipartRequestPayload
            {
                Email = "john@doe.local"
            });

            var attribute = new MultipartRequestAttribute();
            var valueProvider = new MultipartRequestValueProvider<MultipartRequestPayload>(attribute, request, null);

            var val = await valueProvider.GetValueAsync();

            if (val is MultipartRequestData<MultipartRequestPayload> multipartRequestData)
            {
                Assert.AreEqual(0, multipartRequestData.Files.Length);
                Assert.AreEqual("john@doe.local", multipartRequestData.Data.Email);
            }
            else
            {
                Assert.Fail("value is not correct type");
            }
        }

        [TestMethod]
        public async Task GetValueAsync_RequiredDataMissing_Validates()
        {
            var request = GetRequest(new ValidatedMultipartRequestPayload
            {
                Email = "john@doe.local"
            });

            var attribute = new MultipartRequestAttribute {ValidateData = true};
            var valueProvider =
                new MultipartRequestValueProvider<ValidatedMultipartRequestPayload>(attribute, request, null);

            var val = await valueProvider.GetValueAsync();

            if (val is MultipartRequestData<ValidatedMultipartRequestPayload> multipartRequestData)
            {
                Assert.IsFalse(multipartRequestData.IsValid);
                Assert.AreEqual(1, multipartRequestData.ValidationResults.Count());
            }
            else
            {
                Assert.Fail("value is not correct type");
            }
        }


        [TestMethod]
        public async Task GetValueAsync_WithFiles_FindsAllFiles()
        {
            var files = new List<Tuple<HttpContent, string, string>>();
            var bytes = Encoding.UTF8.GetBytes("Hello");
            var bytes2 = Encoding.UTF8.GetBytes("Goodbye");
            files.Add(new Tuple<HttpContent, string, string>(new ByteArrayContent(bytes, 0, bytes.Length), "file",
                "greeting.txt"));
            files.Add(new Tuple<HttpContent, string, string>(new ByteArrayContent(bytes2, 0, bytes2.Length), "file",
                "goodbye.txt"));

            var request = GetRequest(new ValidatedMultipartRequestPayload
            {
                Email = "john@doe.local"
            }, files.ToArray());

            var attribute = new MultipartRequestAttribute();
            var valueProvider =
                new MultipartRequestValueProvider<ValidatedMultipartRequestPayload>(attribute, request, null);

            var val = await valueProvider.GetValueAsync();

            if (val is MultipartRequestData<ValidatedMultipartRequestPayload> multipartRequestData)
            {
                Assert.AreEqual(2, multipartRequestData.Files.Length);
                var fileContent =
                    Encoding.UTF8.GetString(multipartRequestData.Files.First(x => x.FileName == "greeting.txt")
                        .Content);
                Assert.AreEqual("Hello", fileContent);
            }
            else
            {
                Assert.Fail("value is not correct type");
            }
        }

        private HttpRequestMessage GetRequest(object payload, Tuple<HttpContent, string, string>[] files = null)
        {
            var multipart = new MultipartFormDataContent
            {
                new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8,
                    MediaTypeNames.Application.Json)
            };

            if (files != null)
            {
                foreach (var (byteArrayContent, name, filename) in files)
                {
                    multipart.Add(byteArrayContent, name, filename);
                }
            }

            var request = new HttpRequestMessage(HttpMethod.Post, "api/test/multipart")
            {
                Content = multipart
            };

            return request;
        }

        class MultipartRequestPayload
        {
            public string Email { get; set; }
        }

        class ValidatedMultipartRequestPayload
        {
            [Required]
            public string Username { get; set; }

            public string Email { get; set; }
        }

        /*
          var bytes = Encoding.UTF8.GetBytes("Hello");
            var bytes2 = Encoding.UTF8.GetBytes("Goodbye");
            multipart.Add(new ByteArrayContent(bytes, 0, bytes.Length), "file", "greeting.txt");
            multipart.Add(new ByteArrayContent(bytes2, 0, bytes2.Length), "file", "goodbye.txt");

         */
    }
}