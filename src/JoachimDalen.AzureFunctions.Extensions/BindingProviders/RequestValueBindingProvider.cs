using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using JoachimDalen.AzureFunctions.Extensions.Attributes;
using JoachimDalen.AzureFunctions.Extensions.Bindings;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Logging;

namespace JoachimDalen.AzureFunctions.Extensions.BindingProviders
{
    public class RequestValueBindingProvider : IBindingProvider
    {
        private readonly ILogger _logger;

        private static readonly Type[] SupportedTypes =
        {
            typeof(Guid),
            typeof(bool),
            typeof(int),
            typeof(string),
            typeof(object)
        };

        public RequestValueBindingProvider(ILogger logger)
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
            var attribute = context.Parameter.GetCustomAttribute<RequestValueAttribute>();

            if (attribute == null)
            {
                return Task.FromResult<IBinding>(null);
            }

            var isSupportedTypeBinding = BindingHelpers.MatchParameterType(parameter, SupportedTypes);
            var isUserTypeBinding = !isSupportedTypeBinding && BindingHelpers.IsValidUserType(parameter.ParameterType);
            if (!isSupportedTypeBinding && !isUserTypeBinding)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                    "Can't bind RequestValueAttribute to type '{0}'.", parameter.ParameterType));
            }

            var binding =
                (IBinding)Activator.CreateInstance(typeof(RequestValueBinding), _logger, attribute, isUserTypeBinding,
                    context.Parameter);

            return Task.FromResult(binding);
        }
    }
}