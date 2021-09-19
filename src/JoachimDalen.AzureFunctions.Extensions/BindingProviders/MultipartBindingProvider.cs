using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using JoachimDalen.AzureFunctions.Extensions.Attributes;
using JoachimDalen.AzureFunctions.Extensions.Bindings;
using JoachimDalen.AzureFunctions.Extensions.Models;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Logging;

namespace JoachimDalen.AzureFunctions.Extensions.BindingProviders
{
    public class MultipartBindingProvider : IBindingProvider
    {
        private readonly ILogger _logger;

        public MultipartBindingProvider(ILogger logger)
        {
            _logger = logger;
        }

        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var parameter = context.Parameter;
            var attribute = context.Parameter.GetCustomAttribute<MultipartRequestAttribute>();

            if (attribute == null)
            {
                return Task.FromResult<IBinding>(null);
            }

            if (!BindingHelpers.IsOfGenericType(parameter, typeof(MultipartRequestData<>)))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                    "Can't bind MultipartRequestAttribute to type '{0}'.", parameter.ParameterType));
            }

            var dataType = parameter.ParameterType.GetGenericArguments()?.FirstOrDefault();
            if (dataType == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                    "Can't bind MultipartRequestData to type '{0}'.", parameter.ParameterType));
            }

            if (!BindingHelpers.IsValidUserType(dataType))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                    "Can't bind MultipartRequestData to type '{0}'.", dataType));
            }

            var binding = CreateBodyBinding(_logger, dataType, attribute);
            return Task.FromResult(binding);
        }

        private IBinding CreateBodyBinding(ILogger log, Type T, MultipartRequestAttribute attribute)
        {
            var type = typeof(MultipartRequestBinding<>).MakeGenericType(T);
            var a_Context = Activator.CreateInstance(type, log, attribute);
            return (IBinding) a_Context;
        }
    }
}