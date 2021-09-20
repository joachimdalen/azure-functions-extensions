using System.ComponentModel.DataAnnotations;

namespace Integration.FunctionApp.Models
{
    public class BodyWithValidation
    {
        [Required]
        [MinLength(4), MaxLength(24)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}