using System.Net.Http;
using System.Net.Http.Headers;

namespace JoachimDalen.AzureFunctions.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string GetEscapedContentDispositionFileName(this ContentDispositionHeaderValue headerValue)
        {
            return headerValue?.FileName?.Trim('\"');
        }

        public static string GetEscapedContentDispositionName(this ContentDispositionHeaderValue headerValue)
        {
            return headerValue?.Name?.Trim('\"');
        }

        public static bool HasFiles(this HttpContent content, string fileKey)
        {
            return content?.Headers?.ContentDisposition?.GetEscapedContentDispositionName() == fileKey;
        }

        public static bool HasData(this HttpContent content)
        {
            return content?.Headers?.ContentType?.MediaType == "application/json";
        }
    }
}