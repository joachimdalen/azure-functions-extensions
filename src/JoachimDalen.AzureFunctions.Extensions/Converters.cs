using System;

namespace JoachimDalen.AzureFunctions.Extensions
{
    internal static class Converters
    {
        internal static bool TryCreateValue(object input, Type inputType, out object value)
        {
            value = default;
            var convertType = Nullable.GetUnderlyingType(inputType) ?? inputType;

            if (input == null) return default;

            if (convertType == typeof(string))
            {
                value = input.ToString();
                return true;
            }

            if (convertType == typeof(Guid))
            {
                if (!Guid.TryParse(input.ToString(), out Guid guid))
                {
                    return false;
                }

                value = guid;
                return true;
            }

            if (convertType == typeof(int))
            {
                if (!int.TryParse(input.ToString(), out int intVal))
                {
                    return false;
                }

                value = intVal;
                return true;
            }

            return false;
        }
    }
}