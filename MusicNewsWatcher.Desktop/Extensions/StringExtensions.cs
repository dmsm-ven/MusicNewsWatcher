using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace MusicNewsWatcher.Desktop.Extensions;

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
        using var md5 = System.Security.Cryptography.MD5.Create();

        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        return Convert.ToHexString(hashBytes);

    }
}
