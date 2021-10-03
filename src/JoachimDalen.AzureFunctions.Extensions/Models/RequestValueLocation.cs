using System;

namespace JoachimDalen.AzureFunctions.Extensions.Models
{
    [Flags]
    public enum RequestValueLocation
    {
        Query = 1,
        Header = 2
    }
}