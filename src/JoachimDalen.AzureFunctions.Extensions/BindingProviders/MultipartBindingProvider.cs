using System;
using System.Threading.Tasks;
using JoachimDalen.AzureFunctions.Extensions.Bindings;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Logging;

namespace JoachimDalen.AzureFunctions.Extensions.BindingProviders
{
    public class MultipartBindingProvider : IBindingProvider
    {
        private readonly ILogger logger;

        public MultipartBindingProvider(ILogger logger)
        {
            this.logger = logger;
        }

        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            IBinding binding = CreateBodyBinding(logger, context.Parameter.ParameterType);
            return Task.FromResult(binding);
        }

        private IBinding CreateBodyBinding(ILogger log, Type T)
        {
            var type = typeof(MultipartRequestBinding<>).MakeGenericType(T);
            var a_Context = Activator.CreateInstance(type, new object[] {log});
            return (IBinding) a_Context;
        }
    }
}