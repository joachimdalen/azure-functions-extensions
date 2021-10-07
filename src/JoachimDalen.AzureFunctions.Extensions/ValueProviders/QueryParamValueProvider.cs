using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using JoachimDalen.AzureFunctions.Extensions.Abstractions;
using JoachimDalen.AzureFunctions.Extensions.Attributes;
using JoachimDalen.AzureFunctions.Extensions.Models;
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
            if (_isUserTypeBinding)
            {
                var isContainerType = BindingHelpers.IsOfGenericType(_parameter, typeof(QueryParamContainer<>));
                object container = null;
                PropertyInfo[] properties;
                Type containerValueType = null;
                if (isContainerType)
                {
                    containerValueType = _parameter.ParameterType.GetGenericArguments()?.FirstOrDefault();
                    container = Activator.CreateInstance(containerValueType);
                    properties = containerValueType.GetProperties();
                }
                else
                {
                    container = Activator.CreateInstance(_parameter.ParameterType);
                    properties = _parameter.ParameterType.GetProperties();
                }


                foreach (var propertyInfo in properties)
                {
                    var attribute = propertyInfo.GetCustomAttribute<QueryParamValueAttribute>();
                    var name = attribute?.Name ?? propertyInfo.Name;
                    if (!_request.Query.TryGetValue(name, out var queryValues))
                    {
                        continue;
                    }

                    var value = queryValues.First();

                    if (!Converters.TryCreateValue(value, propertyInfo.PropertyType, out var convertedValue))
                    {
                        continue;
                    }

                    if (propertyInfo.CanWrite)
                    {
                        propertyInfo.SetValue(container, convertedValue);
                    }
                }

                if (isContainerType)
                {
                    var type = typeof(QueryParamContainer<>).MakeGenericType(containerValueType);
                    var containerInstance = Activator.CreateInstance(type);

                    if (_attribute.Validate && containerInstance is IValidatable validatable)
                    {
                        validatable.Validate(container);
                    }


                    var param = type.GetProperty("Params");
                    if (param != null && param.CanWrite)
                    {
                        param.SetValue(containerInstance, container);
                    }

                    return containerInstance;
                }

                return container;
            }


            object selectedValue;
            if (!_request.Query.TryGetValue(_attribute.Name, out var values))
            {
                return null;
            }


            if (_parameter.ParameterType.IsArray)
            {
                if (!Converters.TryCreateArray(values.ToArray(), _parameter.ParameterType, out selectedValue))
                {
                    selectedValue = null;
                }
            }
            else
            {
                if (!Converters.TryCreateValue(values.First(), _parameter.ParameterType, out selectedValue))
                {
                    selectedValue = null;
                }
            }

            return selectedValue;
        }

        public Type Type => typeof(object);
        public string ToInvokeString() => string.Empty;
    }
}