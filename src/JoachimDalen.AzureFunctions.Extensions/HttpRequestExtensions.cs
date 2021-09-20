using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using JoachimDalen.AzureFunctions.Extensions.Abstractions;
using JoachimDalen.AzureFunctions.Extensions.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace JoachimDalen.AzureFunctions.Extensions
{
    public static class HttpRequestExtensions
    {
        internal static string GetEscapedContentDispositionFileName(this ContentDispositionHeaderValue headerValue)
        {
            return headerValue?.FileName?.Trim('\"');
        }

        private static string GetEscapedContentDispositionName(this ContentDispositionHeaderValue headerValue)
        {
            return headerValue?.Name?.Trim('\"');
        }

        internal static bool HasFiles(this HttpContent content, string fileKey)
        {
            return content?.Headers?.ContentDisposition?.GetEscapedContentDispositionName() == fileKey;
        }

        internal static bool HasData(this HttpContent content)
        {
            return content?.Headers?.ContentType?.MediaType == "application/json";
        }

        public static async Task<HttpRequestBody<T>> GetValidatedBody<T>(this HttpRequestMessage request)
        {
            var body = new HttpRequestBody<T>();
            var bodyString = await request.Content.ReadAsStringAsync();
            body.Value = string.IsNullOrEmpty(bodyString) ? default : JsonConvert.DeserializeObject<T>(bodyString);

            return GetValidatedBody<T>(bodyString);
        }

        public static async Task<HttpRequestBody<T>> GetValidatedBody<T>(this HttpRequest request)
        {
            var bodyData = new StreamReader(request.Body);
            bodyData.BaseStream.Seek(0, SeekOrigin.Begin);
            var bodyString = bodyData.ReadToEnd();
            return GetValidatedBody<T>(bodyString);
        }

        private static HttpRequestBody<T> GetValidatedBody<T>(string bodyValue)
        {
            var body = new HttpRequestBody<T>();
            var results = new List<ValidationResult>();
            body.Value = string.IsNullOrEmpty(bodyValue) ? default : JsonConvert.DeserializeObject<T>(bodyValue);
            body.IsValid = Validator.TryValidateObject(body.Value, new ValidationContext(body.Value, null, null),
                results, true);
            body.ValidationResults = results;
            return body;
        }

        internal static IValidatable Validate(this IValidatable validatable, object value)
        {
            var results = new List<ValidationResult>();
            validatable.IsValid = Validator.TryValidateObject(value,
                new ValidationContext(value, null, null), results, true);
            validatable.ValidationResults = results;
            return validatable;
        }
    }
}