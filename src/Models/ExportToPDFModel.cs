using Syncfusion.Drawing;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;

namespace iBudget.Models;

public class ExportToPDFModel
{
    public byte[] GetPDF(string url)
    {
        var htmlConverter = InitializeHtmlConverter();
        var document = htmlConverter.Convert(url);

        using (var stream = new MemoryStream())
        {
            document.Save(stream);
            document.Close();

            if (SystemManager.IsDevelopment)
                CreateWatermark(stream);

            return stream.ToArray();
        }
    }

    private HtmlToPdfConverter InitializeHtmlConverter()
    {
        var htmlConverter = new HtmlToPdfConverter();
        var blinkConverterSettings = new BlinkConverterSettings
        {
            PdfPageSize = PdfPageSize.A4,
            EnableHyperLink = false,
            EnableJavaScript = false,
            AdditionalDelay = 0
        };

        blinkConverterSettings.CommandLineArguments.Add("--no-sandbox");
        blinkConverterSettings.CommandLineArguments.Add("--disable-setuid-sandbox");

        htmlConverter.ConverterSettings = blinkConverterSettings;
        return htmlConverter;
    }

    private void CreateWatermark(MemoryStream stream)
    {
        using (var loadedDocument = new PdfLoadedDocument(stream))
        {
            foreach (PdfPageBase page in loadedDocument.Pages)
                CreateWatermark(page);

            loadedDocument.Save(stream);
            loadedDocument.Close(true);
        }
    }

    private void CreateWatermark(PdfPageBase page)
    {
        var graphics = page.Graphics;
        PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20, PdfFontStyle.Bold);
        var state = graphics.Save();

        graphics.SetTransparency(0.60f);
        graphics.RotateTransform(-40);

        graphics.DrawString(
            "Arquivo exclusivamente dedicado para testes. Não tem validade alguma!",
            font,
            PdfBrushes.Red,
            new PointF(-280, 350)
        );

        graphics.Restore(state);
    }
}