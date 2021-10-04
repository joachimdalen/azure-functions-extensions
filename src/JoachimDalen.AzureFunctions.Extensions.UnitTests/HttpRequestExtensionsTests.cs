using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JoachimDalen.AzureFunctions.Extensions.UnitTests
{
    [TestClass]
    public class HttpRequestExtensionsTests
    {
        [DataTestMethod]
        [DataRow("form-data;name=\"file\"; filename=\"fileOne.png\"")]
        [DataRow("form-data;name=\"file\"; filename=fileOne.png")]
        public void GetEscapedContentDispositionFileName_Data_ReturnsCorrect(string contentDisposition)
        {
            var contentDispositionHeaderValue = ContentDispositionHeaderValue.Parse(contentDisposition);
            Assert.AreEqual("fileOne.png", contentDispositionHeaderValue.GetEscapedContentDispositionFileName());
        }

        [TestMethod]
        public async Task HasFiles_Files_ReturnsTrue()
        {
            var bytes = Encoding.UTF8.GetBytes("Hello");
            var multipart = new MultipartFormDataContent
            {
                {new ByteArrayContent(bytes, 0, bytes.Length), "file", "greeting.txt"}
            };

            var mp = (await multipart.ReadAsMultipartAsync()).Contents.First();
            Assert.IsTrue(mp.HasFiles("file"));
        }

        [TestMethod]
        public async Task HasFiles_StringContent_ReturnsFalse()
        {
            var multipart = new MultipartFormDataContent
            {
                new StringContent("Hello", Encoding.UTF8, MediaTypeNames.Text.Plain)
            };

            var mp = (await multipart.ReadAsMultipartAsync()).Contents.First();
            Assert.IsFalse(mp.HasFiles("file"));
        }

        [TestMethod]
        public async Task HasData_JsonContent_ReturnsTrue()
        {
            var multipart = new MultipartFormDataContent
            {
                new StringContent("Hello", Encoding.UTF8, MediaTypeNames.Application.Json)
            };

            var mp = (await multipart.ReadAsMultipartAsync()).Contents.First();
            Assert.IsTrue(mp.HasData());
        }
    }
}