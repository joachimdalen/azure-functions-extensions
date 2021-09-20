using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JoachimDalen.AzureFunctions.Extensions.Abstractions;

namespace JoachimDalen.AzureFunctions.Extensions.Models
{
    public class HttpRequestBody<T> : IValidatable
    {
        public T Value { get; set; }
        public bool IsValid { get; set; }
        public IEnumerable<ValidationResult> ValidationResults { get; set; }
    }
}