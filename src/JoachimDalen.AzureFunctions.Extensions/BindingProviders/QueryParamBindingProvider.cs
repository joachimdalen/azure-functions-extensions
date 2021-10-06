using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using JoachimDalen.AzureFunctions.Extensions.Attributes;
using JoachimDalen.AzureFunctions.Extensions.Bindings;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Logging;
using Microsoft.Extensions.Logging;

namespace JoachimDalen.AzureFunctions.Extensions.BindingProviders
{
    public class QueryParamBindingProvider : IBindingProvider
    {
        private readonly ILogger _logger;

        private static readonly Type[] SupportedTypes =
        {
            typeof(Guid),
            typeof(bool),
            typeof(int),
            typeof(string),
            typeof(object),
            typeof(string[]),
            typeof(int[]),
            typeof(Guid[]),
        };

        public QueryParamBindingProvider(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory != null
                ? loggerFactory.CreateLogger(LogCategories.Bindings)
                : throw new ArgumentNullException(nameof(loggerFactory));
        }

        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var parameter = context.Parameter;
            var attribute = context.Parameter.GetCustomAttribute<QueryParamAttribute>();

            if (attribute == null)
            {
                return Task.FromResult<IBinding>(null);
            }

            var isSupportedTypeBinding = BindingHelpers.MatchParameterType(parameter, SupportedTypes);
            var isUserTypeBinding = !isSupportedTypeBinding && BindingHelpers.IsValidUserType(parameter.ParameterType);
            if (!isSupportedTypeBinding && !isUserTypeBinding)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                    "Can't bind QueryParamAttribute to type '{0}'.", parameter.ParameterType));
            }

            var type = typeof(QueryParamBinding<>).MakeGenericType(context.Parameter.ParameterType);
            var binding =
                (IBinding)Activator.CreateInstance(type, _logger, attribute, isUserTypeBinding, context.Parameter);

            return Task.FromResult(binding);
        }
    }
}