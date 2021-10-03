using System;
using System.Reflection;
using System.Threading.Tasks;
using JoachimDalen.AzureFunctions.Extensions.Attributes;
using JoachimDalen.AzureFunctions.Extensions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Logging;

namespace JoachimDalen.AzureFunctions.Extensions.ValueProviders
{
    public class RequestValueProvider : IValueProvider
    {
        private readonly HttpRequest _request;
        private readonly RequestValueAttribute _attribute;
        private readonly ParameterInfo _parameter;
        private readonly bool _isUserTypeBinding;
        private readonly ILogger _logger;

        public RequestValueProvider(HttpRequest request, RequestValueAttribute attribute, ParameterInfo parameter,
            bool isUserTypeBinding,
            ILogger logger)
        {
            _request = request;
            _attribute = attribute;
            _parameter = parameter;
            _isUserTypeBinding = isUserTypeBinding;
            _logger = logger;
        }

        public async Task<object> GetValueAsync()
        {
            object value = null;

            if (_attribute.Location.HasFlag(RequestValueLocation.Header))
            {
                if (TryGetFromHeader(_attribute.Name, out var val))
                {
                    value = val;
                }
                else
                {
                    if (_attribute.Aliases != null)
                    {
                        foreach (var nameAlias in _attribute.Aliases)
                        {
                            if (TryGetFromHeader(nameAlias, out var aliasVal))
                            {
                                value = aliasVal;
                            }
                        }
                    }
                }
            }

            if (value == null && _attribute.Location.HasFlag(RequestValueLocation.Query))
            {
                if (TryGetFromQuery(_attribute.Name, out var val))
                {
                    value = val;
                }
                else
                {
                    if (_attribute.Aliases != null)
                    {
                        foreach (var nameAlias in _attribute.Aliases)
                        {
                            if (TryGetFromQuery(nameAlias, out var aliasVal))
                            {
                                value = aliasVal;
                            }
                        }
                    }
                }
            }


            if (Converters.TryCreateValue(value, _parameter.ParameterType, out var convertedValue))
            {
                return convertedValue;
            }

            return value;
        }

        private bool TryGetFromHeader(string key, out object val)
        {
            if (_request.Headers.TryGetValue(key, out var vals))
            {
                val = vals;
                return true;
            }

            val = null;
            return false;
        }

        private bool TryGetFromQuery(string key, out object val)
        {
            if (_request.Query.TryGetValue(key, out var vals))
            {
                val = vals;
                return true;
            }

            val = null;
            return false;
        }

        public string ToInvokeString()
        {
            return "My invoke string";
        }

        public Type Type { get; }
    }
}