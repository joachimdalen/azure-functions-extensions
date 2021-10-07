using System;

namespace Integration.FunctionApp.Models
{
    public class NameValueModel
    {
        public string Name { get; set; }
        public string[] Values { get; set; }
        public Guid[] Ids { get; set; }
    }
}