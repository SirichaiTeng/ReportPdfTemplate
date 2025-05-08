using iText.Forms;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Layout;
using iText.Layout.Element;
using static iText.IO.Codec.TiffWriter;

namespace ReportPdfTemplate.Utils;

public static class iTextUtils
{
    public static void SetFormField(PdfAcroForm form, string field, string value, PdfFont font, int fontSize)
    {
        var fieldName = form.GetField(field);
        if (fieldName == null)
        {
            Console.WriteLine($"⚠️ Warning: Field '{field}' not found.");
            return;
        }

        form.GetField(field)
                .SetValue(value)
                .SetFont(font)
                .SetFontSize(fontSize);
        
    }

    public static void AddImage(Document document, string imagePath,int page, float x, float y, float width, float height)
    {
        var qrCodeImage = ImageDataFactory.Create(imagePath);
        var qrCode = new Image(qrCodeImage)
            .SetFixedPosition(page,x, y)
            .ScaleToFit(width, height);
        document.Add(qrCode);
    }
}
