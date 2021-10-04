using JoachimDalen.AzureFunctions.Extensions.Attributes;
using JoachimDalen.AzureFunctions.Extensions.BindingProviders;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.Logging;

namespace JoachimDalen.AzureFunctions.Extensions.ConfigProviders
{
    public class ConfigProvider : IExtensionConfigProvider
    {
        private readonly ILoggerFactory _loggerFactory;

        public ConfigProvider(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public void Initialize(ExtensionConfigContext context)
        {
            context.AddBindingRule<QueryParamAttribute>().Bind(new QueryParamBindingProvider(_loggerFactory));
            context.AddBindingRule<MultipartRequestAttribute>().Bind(new MultipartBindingProvider(_loggerFactory));
            context.AddBindingRule<RequestValueAttribute>().Bind(new RequestValueBindingProvider(_loggerFactory));
        }
    }
}