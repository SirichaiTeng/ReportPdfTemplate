using iText.Forms;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using ReportPdfTemplate.Models;
using ReportPdfTemplate.Utils;
using System.Globalization;

namespace ReportPdfTemplate.Services;

public interface IGenerateDocument
{
    byte[] CreateSaleRequest(RequestDto request);
}

public class GenerateDocument : IGenerateDocument
{
    private IPdfFontFactory pdfFontFactory;
    public GenerateDocument(IPdfFontFactory pdfFont)
    {
        pdfFontFactory = pdfFont;
    }
    public byte[] CreateSaleRequest(RequestDto request)
    {
        // Font paths
        var pathSarabunNew = Path.Combine("Fonts", "THSarabunNew.ttf");
        var pathSarabunNewBold = Path.Combine("Fonts", "THSarabunNew Bold.ttf");
        var pathWingding = Path.Combine("Fonts", "wingding.ttf");

        // Image path
        var pathImageQrCode = Path.Combine("Images", "qrcode.png");
        var pathImageLogo = Path.Combine("Images", "logo.png");

        // Template path
        var templateSale = Path.Combine("Template", "SaleTemplate.pdf");

        // Load fonts
        var fontSarabunNew = pdfFontFactory.CreateFont(pathSarabunNew, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
        var fontSarabunNewBold = pdfFontFactory.CreateFont(pathSarabunNewBold, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
        var fontWingding = pdfFontFactory.CreateFont(pathWingding, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);

        // Create PDF in memory
        var templateByte = File.ReadAllBytes(templateSale);
        using (var outputStream = new MemoryStream())
        using (var inputStream = new MemoryStream(templateByte))
        using (var pdfReader = new PdfReader(inputStream))
        using (var writer = new PdfWriter(outputStream))
        using (var pdfDocument = new PdfDocument(pdfReader, writer))
        using (var document = new Document(pdfDocument))
        {
            // ดึงข้อมูลจากฟอร์ม PDF
            var form = PdfAcroForm.GetAcroForm(pdfDocument, true);

            // เพิ่มข้อมูลลงฟอร์ม
            var filedName = "นาย ทดสอบภาษาไทย testeng";
            var datestart = DateTime.Now.AddHours(7).ToString("d MMMM yyyy", new CultureInfo("th-TH"));

            iTextUtils.SetFormField(form, "Name", filedName, fontSarabunNew, 14);
            iTextUtils.SetFormField(form, "DateStart", datestart, fontSarabunNew, 14);


            // เพิ่มรูปภาพ
            iTextUtils.AddImage(document, pathImageQrCode, 1, 50, 50, 45, 45);
            iTextUtils.AddImage(document, pathImageLogo, 1, 50, 50, 45, 45);

            var headerText = new Paragraph($@"บริษัท ABC จำกัด (มหาชน) เลขที่เอกสาร: {request.Description}")
                .SetFont(fontSarabunNew)
                .SetFontSize(14)
                .SetFixedPosition(100, 750, 400);
            document.Add(headerText);

            // เพิ่มเครื่องติ๊กถูก (chr 252) wingdings
            int total = 1;
            if (total == 1)
            {
                iTextUtils.SetFormField(form, "CheckBox", ((char)252).ToString(), fontWingding, 14);
            }

            // Set PDF Metadata ข้อมูล Metadata เช่น ชื่อเอกสาร, ผู้เขียน, วันที่สร้าง
            pdfDocument.GetDocumentInfo()
                .SetTitle("เอกสารการขาย")
                .SetAuthor("ABC Co., Ltd.")
                .SetCreator("Document Generator")
                .SetSubject("Sales Document")
                .SetKeywords("Sales, PDF, Document");

            // ล็อกฟอร์มไม่ให้แก้ไข
            form.FlattenFields();
            document.Close();
            pdfDocument.Close();
            //var base64string = Convert.ToBase64String(ms.ToArray());
            //return base64string;
            return outputStream.ToArray();
        }

    }

    //public byte[] CreateSaleRequest(RequestDto request)
    //{
    //    // Font paths
    //    var pathSarabunNew = Path.Combine("Fonts", "THSarabunNew.ttf");
    //    var pathSarabunNewBold = Path.Combine("Fonts", "THSarabunNew Bold.ttf");
    //    var pathWingding = Path.Combine("Fonts", "wingding.ttf");

    //    // Image path
    //    var pathImageQrCode = Path.Combine("Images", "qrcode.png");
    //    var pathImageLogo = Path.Combine("Images", "logo.png");

    //    // Template path
    //    var templateSale = Path.Combine("Template", "SaleTemplate.pdf");

    //    // Load fonts
    //    var fontSarabunNew = pdfFontFactory.CreateFont(pathSarabunNew, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
    //    var fontSarabunNewBold = pdfFontFactory.CreateFont(pathSarabunNewBold, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
    //    var fontWingding = pdfFontFactory.CreateFont(pathWingding, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);

    //    // Create PDF in memory
    //    using (var ms = new MemoryStream())
    //    {
    //        using (var pdfReader = new PdfReader(templateSale))
    //        using (var writer = new PdfWriter(ms))
    //        using (var pdfDocument = new PdfDocument(pdfReader, writer))
    //        using (var document = new Document(pdfDocument))
    //        {
    //            // ดึงข้อมูลจากฟอร์ม PDF
    //            var form = PdfAcroForm.GetAcroForm(pdfDocument, true);

    //            // เพิ่มข้อมูลลงฟอร์ม
    //            var filedName = "นาย ทดสอบภาษาไทย testeng";
    //            var datestart = DateTime.Now.AddHours(7).ToString("d MMMM yyyy", new CultureInfo("th-TH"));

    //            iTextUtils.SetFormField(form, "Name", filedName, fontSarabunNew, 14);
    //            iTextUtils.SetFormField(form, "DateStart", datestart, fontSarabunNew, 14);


    //            // เพิ่มรูปภาพ
    //            iTextUtils.AddImage(document, pathImageQrCode, 1, 50, 50, 45, 45);
    //            iTextUtils.AddImage(document, pathImageLogo, 1, 50, 50, 45, 45);

    //            // เพิ่มเครื่องติ๊กถูก (chr 252) wingdings
    //            int total = 1;
    //            if (total == 1)
    //            {
    //                iTextUtils.SetFormField(form, "CheckBox", ((char)252).ToString(), fontWingding, 14);
    //            }

    //            // Set PDF Metadata ข้อมูล Metadata เช่น ชื่อเอกสาร, ผู้เขียน, วันที่สร้าง
    //            pdfDocument.GetDocumentInfo()
    //                .SetTitle("เอกสารการขาย")
    //                .SetAuthor("ABC Co., Ltd.")
    //                .SetCreator("Document Generator")
    //                .SetSubject("Sales Document")
    //                .SetKeywords("Sales, PDF, Document");

    //            //// ล็อกฟอร์มไม่ให้แก้ไข
    //            form.FlattenFields();
    //        }
    //        return ms.ToArray();
    //    }
    //}
}
