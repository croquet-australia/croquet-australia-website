using EmptyStringGuard;
using NullGuard;

namespace CroquetAustralia.Library.Infrastructure
{
    public static class StringExtensions
    {
        public static IMarkdownString AsMarkdown(this string value)
        {
            return new MarkdownString(value);
        }

        [return: AllowNull]
        public static string AsTrimmedString([AllowNull, AllowEmpty] this object value)
        {
            return value?.ToString().Trim();
        }

        public static string TrimSlashes([AllowNull, AllowEmpty] this string value)
        {
            return value.AsTrimmedString().Trim('/').Trim('\\');
        }
    }
}