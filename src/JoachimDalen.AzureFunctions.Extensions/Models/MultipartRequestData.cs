using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JoachimDalen.AzureFunctions.Extensions.Abstractions;

namespace JoachimDalen.AzureFunctions.Extensions.Models
{
    public class MultipartRequestData<T> : IValidatable
    {
        public T Data { get; set; }
        public MultipartFile[] Files { get; set; }
        public bool IsValid { get; set; }
        public IEnumerable<ValidationResult> ValidationResults { get; set; }
    }
}