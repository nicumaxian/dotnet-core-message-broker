using System.Text.RegularExpressions;

namespace Utils.Extensions
{
    public static class RegexExtensions
    {
        public static string GlobToRegex(this string pattern)
        {
            return Regex.Escape(pattern)
                .Replace(@"\*", ".*")
                .Replace(@"\?", ".");
        }
    }
}