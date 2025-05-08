using iText.Forms.Fields;
using iText.Forms;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using Moq;
using ReportPdfTemplate.Models;
using ReportPdfTemplate.Services;

namespace ReportPdfTemplate.Tests.Controllers;
public class ReportControllerTests
{
    [Fact]
    public async Task CreateDocumentSale_ReturnsOk_WithBase64String()
    {
        // Arrange
        var mockFontFactory = new Mock<IPdfFontFactory>();
        var mockDocumentFactory = new Mock<IPdfDocumentFactory>();

        var fakeFont = PdfFontFactory.CreateFont();
        mockFontFactory
            .Setup(f => f.CreateFont(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PdfFontFactory.EmbeddingStrategy>()))
            .Returns(fakeFont);

             // ต้องใช้ไฟล์ PDF จริงในการ Test เพราะจะต้องใช้ Field ใน PDF Form
        var templatePath = Path.Combine("Template", "SaleTemplate.pdf");
        var inputTemplateStream = File.OpenRead(templatePath);
        using (var ms = new MemoryStream())
        {
            var render = new PdfReader(inputTemplateStream);
            var writer = new PdfWriter(ms);
            var fakePdfDocument = new PdfDocument(new PdfWriter(new MemoryStream()));
            mockDocumentFactory
                .Setup(d => d.CreatePdfDocument(It.IsAny<Stream>(), It.IsAny<string>()))
                .Returns(fakePdfDocument);
        }

        var service = new GenerateDocument(mockFontFactory.Object);
        var mockData = new RequestDto();

        // Act
        var result = service.CreateSaleRequest(mockData);

        // Assert
        Assert.NotEmpty(result);
        Assert.NotEmpty(result);
    }
}

