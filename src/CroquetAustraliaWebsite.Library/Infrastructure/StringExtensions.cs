using EmptyStringGuard;
using NullGuard;

namespace CroquetAustraliaWebsite.Library.Infrastructure
{
    public static class StringExtensions
    {
        [return: AllowNull]
        public static string AsTrimmedString([AllowNull, AllowEmpty] this object value)
        {
            return value == null ? null : value.ToString().Trim();
        }
    }
}
