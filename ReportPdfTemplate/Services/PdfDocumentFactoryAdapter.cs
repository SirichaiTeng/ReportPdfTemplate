using iText.Kernel.Pdf;

namespace ReportPdfTemplate.Services;
public interface IPdfDocumentFactory
{
    PdfDocument CreatePdfDocument(Stream input, string outputPath);
}
public class PdfDocumentFactoryAdapter : IPdfDocumentFactory
{
    public PdfDocument CreatePdfDocument(Stream input, string outputPath)
    {
        var reader = new PdfReader(input);
        var writer = new PdfWriter(outputPath);
        return new PdfDocument(reader, writer);
    }
} 