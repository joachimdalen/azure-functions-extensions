using System.Threading.Tasks;
using JoachimDalen.AzureFunctions.Extensions.ValueProviders;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Extensions.Logging;

namespace JoachimDalen.AzureFunctions.Extensions.Bindings
{
    public class QueryParamBinding<T> : IBinding
    {
        private readonly ILogger _logger;
        private readonly string _queryParamKey;

        public QueryParamBinding(ILogger logger, string name)
        {
            _logger = logger;
            _queryParamKey = name;
        }

        public Task<IValueProvider> BindAsync(BindingContext context)
        {
            // Get the HTTP request
            var request = context.BindingData["req"] as HttpRequest;
            return Task.FromResult<IValueProvider>(new QueryParamValueProvider<T>(request, _queryParamKey, _logger));
        }

        public bool FromAttribute => true;


        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context)
        {
            return null;
        }

        public ParameterDescriptor ToParameterDescriptor() => new ParameterDescriptor();
    }
}