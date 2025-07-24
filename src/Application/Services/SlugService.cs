using Application.Interfaces;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Application.Services;

public class SlugService : ISlugService
{
    public string GenerateSlug(string phrase)
    {
        if (string.IsNullOrWhiteSpace(phrase))
            return string.Empty;

        phrase = NormalizePersianChars(phrase);
        phrase = phrase.ToLowerInvariant();
        phrase = RemoveDiacritics(phrase);
        phrase = Regex.Replace(phrase, @"[^a-z0-9\u0600-\u06FF\s-]", "");
        phrase = Regex.Replace(phrase, @"\s+", "-").Trim('-');

        return phrase;
    }

    private static string NormalizePersianChars(string text)
    {
        return text
            .Replace('ي', 'ی')
            .Replace('ك', 'ک')
            .Replace("ٔ", "")
            .Replace("‌", " ");
    }

    private static string RemoveDiacritics(string text)
    {
        var normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (var c in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}
