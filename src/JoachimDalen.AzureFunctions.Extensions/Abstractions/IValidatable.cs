using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JoachimDalen.AzureFunctions.Extensions.Abstractions
{
    public interface IValidatable
    {
        bool IsValid { get; set; }
        IEnumerable<ValidationResult> ValidationResults { get; set; }
    }
}