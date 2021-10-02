using System;
using System.Reflection;
using System.Threading.Tasks;
using JoachimDalen.AzureFunctions.Extensions.Attributes;
using JoachimDalen.AzureFunctions.Extensions.ValueProviders;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Extensions.Logging;

namespace JoachimDalen.AzureFunctions.Extensions.Bindings
{
    public class RequestValueBinding : IBinding
    {
        private readonly ILogger _logger;
        private readonly RequestValueAttribute _attribute;
        private readonly ParameterInfo _parameter;
        private readonly bool _isUserTypeBinding;

        public RequestValueBinding(ILogger logger, RequestValueAttribute attribute, bool isUserTypeBinding,
            ParameterInfo parameter)
        {
            _logger = logger;
            _attribute = attribute;
            _isUserTypeBinding = isUserTypeBinding;
            _parameter = parameter;
        }


        public async Task<IValueProvider> BindAsync(object value, ValueBindingContext context)
        {
            return null;
        }

        public Task<IValueProvider> BindAsync(BindingContext context)
        {
            if (!context.BindingData.ContainsKey(Constants.DefaultRequestKey))
            {
                throw new InvalidOperationException("Failed to find request in binding context");
            }

            var request = context.BindingData[Constants.DefaultRequestKey] as HttpRequest;
            return Task.FromResult<IValueProvider>(new RequestValueProvider (request, _attribute, _parameter,
                _isUserTypeBinding, _logger));
        }

        public ParameterDescriptor ToParameterDescriptor()
            => new ParameterDescriptor();

        public bool FromAttribute => true;
    }
}