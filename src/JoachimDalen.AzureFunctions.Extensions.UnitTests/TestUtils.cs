using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace JoachimDalen.AzureFunctions.Extensions.UnitTests
{
    internal static class TestUtils
    {
        internal static HttpRequest GetRequest(HttpMethod method, string url, object payload,
            Dictionary<string, string> headers = null, Dictionary<string, StringValues> query = null)
        {
            var context = new DefaultHttpContext
            {
                Request =
                {
                    Path = url,

                    Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload))),
                    Method = method.ToString()
                },
            };

            if (headers != null)
            {
                foreach (var (key, value) in headers)
                {
                    context.Request.Headers.Add(key, value);
                }
            }

            if (query != null)
            {
                context.Request.Query = new QueryCollection(query);
            }

            return context.Request;
        }

        internal static HttpRequestMessage GetRequestMessage(HttpMethod method, string url, object payload,
            Dictionary<string, string> headers)
        {
            var request = new HttpRequestMessage(method, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8,
                    MediaTypeNames.Application.Json)
            };

            if (headers != null)
            {
                foreach (var (key, value) in headers)
                {
                    request.Headers.Add(key, value);
                }
            }

            return request;
        }

        internal static HttpRequestMessage GetMultipartRequest(object payload,
            Tuple<HttpContent, string, string>[] files = null)
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
    }
}