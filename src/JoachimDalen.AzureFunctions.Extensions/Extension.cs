using System;
using JoachimDalen.AzureFunctions.Extensions.ConfigProviders;
using Microsoft.Azure.WebJobs;

namespace JoachimDalen.AzureFunctions.Extensions
{
    public static class Extension
    {
        public static IWebJobsBuilder AddCustomExtensions(this IWebJobsBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddExtension<QueryParamBindingConfigProvider>();
            return builder;
        }
    }
}