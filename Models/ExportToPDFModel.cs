using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;

namespace getQuote.Models;

public class ExportToPDFModel
{
    public byte[] GetPDF(string url)
    {
        HtmlToPdfConverter htmlConverter = new();
        BlinkConverterSettings blinkConverterSettings = new() { PdfPageSize = PdfPageSize.A4 };
        //blinkConverterSettings.ViewPortSize = new Syncfusion.Drawing.Size(1440, 0);
        htmlConverter.ConverterSettings = blinkConverterSettings;

        PdfDocument document = htmlConverter.Convert(url);
        MemoryStream stream = new();
        document.Save(stream);
        document.Close();

        return stream.ToArray();
    }
}
