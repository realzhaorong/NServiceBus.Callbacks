﻿namespace NServiceBus
{
    using System;

    static class CallbackResponseHelper
    {
        public static string ConvertToReturnCode(this object returnCode)
        {
            var statusType = returnCode.GetType();
            if (!statusType.IsIntOrEnum())
            {
                throw new ArgumentException("The status can only be an enum or an integer.", nameof(returnCode));
            }

            var result = returnCode.ToString();
            if (statusType.IsEnum())
            {
                result = Enum.Format(statusType, returnCode, "D");
            }

            return result;
        }

        public static object ConvertFromReturnCode(this string returnCode, Type destinationType)
        {
            if (destinationType == typeof(int))
            {
                return Convert.ToInt32(returnCode);
            }

            if (destinationType == typeof(short))
            {
                return Convert.ToInt16(returnCode);
            }

            if (destinationType == typeof(long))
            {
                return Convert.ToInt64(returnCode);
            }

            if (destinationType.IsEnum())
            {
                return Enum.Parse(destinationType, returnCode);
            }

            throw new ArgumentException("The return code can only be an enum or an integer.", nameof(destinationType));
        }
    }
}