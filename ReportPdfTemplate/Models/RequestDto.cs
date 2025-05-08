namespace ReportPdfTemplate.Models;

public class RequestDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Grad { get; set; }
    public List<Item>? Item { get; set; }

}

public class Item
{
    public string? Name { get; set; }
    public int Price { get; set; }
}
