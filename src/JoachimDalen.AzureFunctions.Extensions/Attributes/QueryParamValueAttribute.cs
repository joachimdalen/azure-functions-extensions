using System;

namespace JoachimDalen.AzureFunctions.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class QueryParamValueAttribute : Attribute
    {
        public string Name { get; }

        public QueryParamValueAttribute(string name)
        {
            Name = name;
        }

        public QueryParamValueAttribute()
        {
        }
    }
}