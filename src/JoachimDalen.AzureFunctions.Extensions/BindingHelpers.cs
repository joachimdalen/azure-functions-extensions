using System;
using System.Linq;
using System.Reflection;

namespace JoachimDalen.AzureFunctions.Extensions
{
    internal static class BindingHelpers
    {
        internal static bool MatchParameterType(ParameterInfo parameter, Type[] types)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            if (types == null)
            {
                throw new ArgumentNullException(nameof(types));
            }

            if (parameter.IsOut)
            {
                return types.Any(p => p.MakeByRefType() == parameter.ParameterType);
            }

            return types.Contains(parameter.ParameterType);
        }

        internal static bool IsValidUserType(Type type)
        {
            return !type.IsInterface && !type.IsPrimitive && type.Namespace != "System";
        }

        internal static bool IsOfGenericType(ParameterInfo parameter, Type generic)
        {
            return parameter.ParameterType.GetGenericTypeDefinition() == generic;
        }
    }
}