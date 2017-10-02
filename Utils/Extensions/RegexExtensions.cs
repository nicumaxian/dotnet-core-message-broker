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
        
        public static bool MatchesGlob(this string str,string glob)
        {
            var regex = new Regex(glob.GlobToRegex());
            return regex.IsMatch(str);
        }
    }
}