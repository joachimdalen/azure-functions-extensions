using JoachimDalen.AzureFunctions.Extensions.Attributes;
using JoachimDalen.AzureFunctions.Extensions.BindingProviders;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.Logging;

namespace JoachimDalen.AzureFunctions.Extensions.ConfigProviders
{
    public class QueryParamBindingConfigProvider : IExtensionConfigProvider
    {
        private readonly ILogger _logger;

        public QueryParamBindingConfigProvider(ILogger logger)
        {
            _logger = logger;
        }

        public void Initialize(ExtensionConfigContext context)
        {
            context.AddBindingRule<QueryParamAttribute>().Bind(new QueryParamBindingProvider(_logger));
        }
    }
}