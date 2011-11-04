using System;
using System.Globalization;

namespace TelnetMock.CrossCutting.Validation
{
    public static class ValidationExtender
    {
        public static Validation<T> IsNotNull<T>(this Validation<T> item) where T : class
        {
            if (item.Value == null)
                throw new ArgumentNullException(item.ArgName);

            return item;
        }

        public static Validation<string> IsNotNullOrEmpty(this Validation<string> item)
        {
            if (string.IsNullOrEmpty(item.Value))
                throw new ArgumentNullException(item.ArgName);

            return item;
        }

        public static Validation<DateTime> IsBetween(this Validation<DateTime> item, DateTime minValue, DateTime maxValue)
        {
            if ((item.Value < minValue) || (item.Value > maxValue))
                throw new ArgumentOutOfRangeException(
                      string.Format(CultureInfo.CurrentCulture, "Parameter {0} must be between {1} and {2}",
                      item.ArgName, minValue, maxValue));

            return item;
        }
    }
}
