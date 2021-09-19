using System;

namespace JoachimDalen.AzureFunctions.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class QueryParamValue : Attribute
    {
        public string Name { get; }

        public QueryParamValue(string name)
        {
            Name = name;
        }

        public QueryParamValue()
        {
        }
    }
}