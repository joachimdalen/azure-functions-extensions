using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using JoachimDalen.AzureFunctions.Extensions.Attributes;
using JoachimDalen.AzureFunctions.Extensions.Bindings;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Logging;

namespace JoachimDalen.AzureFunctions.Extensions.BindingProviders
{
    public class QueryParamBindingProvider : IBindingProvider
    {
        private readonly ILogger _logger;

        private static readonly Type[] SupportedTypes = {
            typeof(Guid),
            typeof(bool),
            typeof(int),
            typeof(string),
            typeof(object),
        };

        public QueryParamBindingProvider(ILogger logger)
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
            var attribute = context.Parameter.GetCustomAttribute<QueryParamAttribute>();

            if (attribute == null)
            {
                return Task.FromResult<IBinding>(null);
            }

            var isSupportedTypeBinding = MatchParameterType(parameter, SupportedTypes);
            var isUserTypeBinding = !isSupportedTypeBinding && IsValidUserType(parameter.ParameterType);
            if (!isSupportedTypeBinding && !isUserTypeBinding)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                    "Can't bind QueryParamAttribute to type '{0}'.", parameter.ParameterType));
            }

            var binding = CreateBodyBinding(_logger, context.Parameter.ParameterType, attribute);
            return Task.FromResult(binding);
        }

        private IBinding CreateBodyBinding(ILogger log, Type T, QueryParamAttribute attribute)
        {
            var type = typeof(QueryParamBinding<>).MakeGenericType(T);
            var aContext = Activator.CreateInstance(type, log, attribute.Name);
            return (IBinding) aContext;
        }

        private bool MatchParameterType(ParameterInfo parameter, IEnumerable<Type> types)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            if (types == null)
            {
                throw new ArgumentNullException(nameof(types));
            }

            if (parameter.IsOut)
            {
                return types.Any(p => p.MakeByRefType() == parameter.ParameterType);
            }

            return types.Contains(parameter.ParameterType);
        }

        private bool IsValidUserType(Type type)
        {
            return !type.IsInterface && !type.IsPrimitive && type.Namespace != "System";
        }
    }
}