using System;
using System.Collections.Generic;
using System.Linq;

namespace JoachimDalen.AzureFunctions.Extensions
{
    internal static class Converters
    {
        internal static bool TryCreateArray(object[] input, Type inputType, out object value)
        {
            value = default;
            var convertType = Nullable.GetUnderlyingType(inputType.GetElementType()) ?? inputType.GetElementType();
            if (input == null) return default;

            if (convertType == typeof(Guid))
            {
                var gList = new List<Guid>();

                foreach (var o in input)
                {
                    if (Guid.TryParse(o as string, out var gid))
                    {
                        gList.Add(gid);
                    }
                }

                value = gList.ToArray();
                return true;
            }

            if (convertType == typeof(string))
            {
                value = input.Select(x => x.ToString()).ToArray();
                return true;
            }

            if (convertType == typeof(int))
            {
                var intList = new List<int>();

                foreach (var o in input)
                {
                    if (int.TryParse(o as string, out var intVal))
                    {
                        intList.Add(intVal);
                    }
                }

                value = intList.ToArray();
            }

            return false;
        }

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