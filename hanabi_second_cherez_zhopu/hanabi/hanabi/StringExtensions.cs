namespace hanabi
{
    public static class StringExtensions
    {
        public static string Capitalize(this string s)
        {
            if (string.IsNullOrEmpty(s) || !char.IsLower(s[0]))
                return s;
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}
