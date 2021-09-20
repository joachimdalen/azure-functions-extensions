using System;
using System.ComponentModel.DataAnnotations;
using JoachimDalen.AzureFunctions.Extensions.Attributes;

namespace Integration.FunctionApp.Models
{
    public class QueryParamValuesWithValidation
    {
        [QueryParamValue]
        public Guid? Id { get; set; }
        
        [QueryParamValue]
        [Required]
        public string Name { get; set; }

        [QueryParamValue]
        [Required]
        public int Age { get; set; }
        
        [QueryParamValue]
        [Required]
        public string Info { get; set; }
    }
}