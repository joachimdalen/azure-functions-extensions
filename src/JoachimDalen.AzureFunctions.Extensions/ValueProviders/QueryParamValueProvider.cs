using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Logging;

namespace JoachimDalen.AzureFunctions.Extensions.ValueProviders
{
    public class QueryParamValueProvider<T> : IValueProvider
    {
        private readonly HttpRequest _request;
        private readonly string _queryParamKey;
        private readonly ILogger _logger;

        public QueryParamValueProvider(HttpRequest request, string queryParamKey, ILogger logger)
        {
            _request = request;
            _queryParamKey = queryParamKey;
            _logger = logger;
        }

        public async Task<object> GetValueAsync()
        {
            try
            {
                if (!_request.Query.TryGetValue(_queryParamKey, out var values))
                {
                    return null;
                }

                var value = values.First();

                if (!typeof(T).IsAssignableFrom(value.GetType()))
                {
                    return null;
                }

                return value;
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