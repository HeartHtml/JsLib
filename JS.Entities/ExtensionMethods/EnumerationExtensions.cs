using System;
using System.Globalization;

namespace JS.Entities.ExtensionMethods
{
    /// <summary>
    /// Encapsulates all logic for Enumeration extension methods
    /// </summary>
    public static class EnumerationExtensions
    {
       
        /// <summary>
        /// Returns the enumeration value as a different formatted string
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string ToFormattedString(this Enum enumValue)
        {
            Type underlyingType = enumValue.GetType();

            return enumValue.ToString();
        }
    }
}
