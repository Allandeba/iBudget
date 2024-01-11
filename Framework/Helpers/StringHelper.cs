using System.Text.RegularExpressions;
using System.Text;

namespace iBudget.Framework.Helpers;

public static class StringHelper
{
    public static string Unaccent(this string content)
    {
        string normalized = Regex.Replace(
            content.Normalize(NormalizationForm.FormD),
            @"[\p{Mn}]",
            ""
        );
        return normalized.Normalize(NormalizationForm.FormC);
    }
}
