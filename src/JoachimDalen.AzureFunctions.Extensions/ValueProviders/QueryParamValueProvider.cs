using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using JoachimDalen.AzureFunctions.Extensions.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Logging;

namespace JoachimDalen.AzureFunctions.Extensions.ValueProviders
{
    public class QueryParamValueProvider<T> : IValueProvider
    {
        private readonly HttpRequest _request;
        private readonly QueryParamAttribute _attribute;
        private readonly ParameterInfo _parameter;
        private readonly bool _isUserTypeBinding;
        private readonly ILogger _logger;

        public QueryParamValueProvider(HttpRequest request, QueryParamAttribute attribute, ParameterInfo parameter,
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
            try
            {
                if (_isUserTypeBinding)
                {
                    var container = Activator.CreateInstance(_parameter.ParameterType);
                    var properties = _parameter.ParameterType.GetProperties();

                    foreach (var propertyInfo in properties)
                    {
                        var attribute = propertyInfo.GetCustomAttribute<QueryParamValue>();
                        var name = attribute?.Name ?? propertyInfo.Name;
                        if (!_request.Query.TryGetValue(name, out var queryValues))
                        {
                            continue;
                        }

                        var value = queryValues.First();

                        if (!TryCreateValue(value, propertyInfo.PropertyType, out var convertedValue))
                        {
                            continue;
                        }

                        if (propertyInfo.CanWrite)
                        {
                            propertyInfo.SetValue(container, convertedValue);
                        }
                    }

                    return container;
                }


                if (!_request.Query.TryGetValue(_attribute.Name, out var values))
                {
                    return null;
                }

                var rawValue = values.First();

                if (!typeof(T).IsAssignableFrom(rawValue.GetType()))
                {
                    return null;
                }

                return rawValue;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Error deserializing object from body");
                throw ex;
            }
        }

        public Type Type => typeof(object);
        public string ToInvokeString() => string.Empty;

        private bool TryCreateValue(object input, Type inputType, out object value)
        {
            value = default;
            var convertType = Nullable.GetUnderlyingType(inputType) ?? inputType;

            if (input == null) return default;

            if (convertType == typeof(string))
            {
                value = input.ToString();
                return true;
            }

            if (convertType == typeof(Guid))
            {
                if (!Guid.TryParse(input.ToString(), out Guid guid))
                {
                    return false;
                }

                value = guid;
                return true;
            }

            if (convertType == typeof(int))
            {
                if (!int.TryParse(input.ToString(), out int intVal))
                {
                    return false;
                }

                value = intVal;
                return true;
            }

            return false;
        }
    }
}