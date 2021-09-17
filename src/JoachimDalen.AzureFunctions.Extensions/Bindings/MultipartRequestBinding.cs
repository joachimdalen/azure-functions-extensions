using System;
using System.Threading.Tasks;
using JoachimDalen.AzureFunctions.Extensions.Attributes;
using JoachimDalen.AzureFunctions.Extensions.ValueProviders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.WebApiCompatShim;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Extensions.Logging;

namespace JoachimDalen.AzureFunctions.Extensions.Bindings
{
    public class MultipartRequestBinding<T> : IBinding
    {
        private readonly ILogger _logger;
        private readonly MultipartRequestAttribute _attribute;

        public MultipartRequestBinding(ILogger logger, MultipartRequestAttribute attribute)
        {
            _logger = logger;
            _attribute = attribute;
        }

        public Task<IValueProvider> BindAsync(BindingContext context)
        {
            if (!context.BindingData.ContainsKey(Constants.DefaultRequestKey))
            {
                throw new InvalidOperationException("Failed to find request in binding context");
            }

            if (!context.BindingData.TryGetValue(Constants.DefaultRequestKey, out var requestObject))
            {
                throw new InvalidOperationException("Failed to find request in binding context");
            }

            if (!(requestObject is HttpRequest request))
            {
                throw new InvalidOperationException("Failed to find request in binding context");
            }

            var requestMessage = new HttpRequestMessageFeature(request.HttpContext).HttpRequestMessage;
            return Task.FromResult<IValueProvider>(new MultipartRequestValueProvider<T>(_attribute, requestMessage, _logger));
        }

        public bool FromAttribute => true;


        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context)
        {
            return null;
        }

        public ParameterDescriptor ToParameterDescriptor() => new ParameterDescriptor();
    }
}