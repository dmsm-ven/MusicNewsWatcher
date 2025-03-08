using System.Text.RegularExpressions;
using System.Web;

namespace MusicNewsWatcher.Core.Extensions;

public static class StringExtensions
{
    private static readonly char[] invad_characters = Path.GetInvalidFileNameChars().Concat(Path.GetInvalidPathChars()).Distinct().ToArray();

    public static string ToDisplayName(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return "<Нет данных>";
        }


        string step1 = HttpUtility.HtmlDecode(input);
        string step2 = Regex.Replace(step1, @"\s{2,}", " ").Trim();

        return step2;
    }

    public static string RemoveInvalidCharacters(this string input)
    {
        if (string.IsNullOrWhiteSpace(input)) { return input; }
        if (input.IndexOfAny(invad_characters) == -1) { return input; }

        string clearName = input;
        foreach (char c in invad_characters)
        {
            clearName = clearName.Replace(c.ToString(), "_");
        }
        return clearName;
    }

    public static string GetMD5(this string input)
    {
        return CreateMD5(input ?? string.Empty);
    }

    public static string CreateMD5(string input)
    {
        // Use input string to calculate MD5 hash
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            return Convert.ToHexString(hashBytes); // .NET 5 +

            // Convert the byte array to hexadecimal string prior to .NET 5
            // StringBuilder sb = new System.Text.StringBuilder();
            // for (int i = 0; i < hashBytes.Length; i++)
            // {
            //     sb.Append(hashBytes[i].ToString("X2"));
            // }
            // return sb.ToString();
        }
    }
}
