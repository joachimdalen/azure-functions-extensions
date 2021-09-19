using System;
using JoachimDalen.AzureFunctions.Extensions.Attributes;

namespace Integration.FunctionApp.Models
{
    public class QueryParamValues
    {
        [QueryParamValue]
        public Guid? Id { get; set; }

        [QueryParamValue]
        public string Name { get; set; }

        [QueryParamValue]
        public int Age { get; set; }
    }
}