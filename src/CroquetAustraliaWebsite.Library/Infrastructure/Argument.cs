using System;
using EmptyStringGuard;
using NullGuard;

namespace CroquetAustraliaWebsite.Library.Infrastructure
{
    public static class Argument
    {
        public static void CannotBeNullOrWhitespace(string paramName, [AllowNull, AllowEmpty]string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", paramName);
            }
        }

        public static void CannotBeNull(string paramName, [AllowNull, AllowEmpty]string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("paramName");
            }
        }
    }
}
