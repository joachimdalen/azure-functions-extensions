using System.ComponentModel.DataAnnotations;

namespace Integration.FunctionApp.Models
{
    public class MultipartRequestBodyData
    {
        [Required]
        public string Username { get; set; }

        public string Email { get; set; }
    }
}