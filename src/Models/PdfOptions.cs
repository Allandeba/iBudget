namespace iBudget.Models;

public sealed class PdfOptions
{
    public const string SectionName = "Pdf";

    public int NavigationTimeoutSeconds { get; set; } = 30;
    public int MaxConcurrentPages { get; set; } = 2;
}
