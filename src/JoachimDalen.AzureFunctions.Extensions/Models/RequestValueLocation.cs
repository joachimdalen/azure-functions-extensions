using System;

namespace JoachimDalen.AzureFunctions.Extensions.Models
{
    [Flags]
    public enum RequestValueLocation
    {
        Query = 0,
        Header = 1
    }
}