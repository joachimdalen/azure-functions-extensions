using System;
using Microsoft.Azure.WebJobs.Description;

namespace JoachimDalen.AzureFunctions.Extensions.Attributes
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    public sealed class QueryParamAttribute : Attribute
    {
        public QueryParamAttribute()
        {
        }

        public QueryParamAttribute(string name)
        {
            Name = name;
        }

        [AutoResolve]
        public string Name { get; set; }
    }
}