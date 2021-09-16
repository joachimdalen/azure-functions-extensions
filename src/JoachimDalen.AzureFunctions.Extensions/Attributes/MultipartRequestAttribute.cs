using System;
using Microsoft.Azure.WebJobs.Description;

namespace JoachimDalen.AzureFunctions.Extensions.Attributes
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class MultipartRequestAttribute : Attribute
    {
        public Type BodyType { get; set; }
        public string FileName { get; set; } = "file";

        /// <summary>
        /// Validate body data
        /// </summary>
        public bool ValidateData { get; set; }
    }
}