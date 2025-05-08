using iText.Kernel.Font;

namespace ReportPdfTemplate.Services;
public interface IPdfFontFactory
{
    PdfFont CreateFont(string path, string encoding, PdfFontFactory.EmbeddingStrategy embeddingStrategy);
}
public class PdfFontFactoryAdapter : IPdfFontFactory
{
    public PdfFont CreateFont(string path, string encoding, PdfFontFactory.EmbeddingStrategy embeddingStrategy)
    {
        return PdfFontFactory.CreateFont(path, encoding, embeddingStrategy);
    }
}
