using System;
using JoachimDalen.AzureFunctions.Extensions.Models;
using Microsoft.Azure.WebJobs.Description;

namespace JoachimDalen.AzureFunctions.Extensions.Attributes
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter)]
    public class RequestValueAttribute : Attribute
    {
        /// <summary>
        /// Location to look for value
        /// </summary>
        public RequestValueLocation Location { get; set; }
        
        /// <summary>
        /// Primary name of value
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Secondary names to look for when first does not match.
        /// </summary>
        public string[] Aliases { get; set; }
    }
}