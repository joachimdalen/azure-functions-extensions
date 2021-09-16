using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace JoachimDalen.AzureFunctions.Extensions.ValueProviders
{
    public class MultipartRequestValueProvider<T> : IValueProvider
    {
        private readonly HttpRequest _request;
        private readonly ILogger _logger;

        public MultipartRequestValueProvider(HttpRequest request, ILogger logger)
        {
            _request = request;
            _logger = logger;
        }

        public async Task<object> GetValueAsync()
        {
            try
            {
                string requestBody = await new StreamReader(this._request.Body).ReadToEndAsync();
                T result = JsonConvert.DeserializeObject<T>(requestBody);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Error deserializing object from body");

                throw ex;
            }
        }

        public Type Type => typeof(object);
        public string ToInvokeString() => string.Empty;
    }
}