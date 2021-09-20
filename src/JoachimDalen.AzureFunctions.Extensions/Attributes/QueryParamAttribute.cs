using System;
using Microsoft.Azure.WebJobs.Description;

namespace JoachimDalen.AzureFunctions.Extensions.Attributes
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    public sealed class QueryParamAttribute : Attribute
    {
        public QueryParamAttribute(string name, bool validate)
        {
            Name = name;
            Validate = validate;
        }

        public QueryParamAttribute() : this(null, false)
        {
        }

        public QueryParamAttribute(string name) : this(name, false)
        {
        }

        public QueryParamAttribute(bool validate) : this(null, validate)
        {
        }

        [AutoResolve]
        public string Name { get; }

        public bool Validate { get; }
    }
}