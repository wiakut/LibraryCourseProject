namespace LibrarySystem.Application.Features.Reports.Dtos;
public class BookInventoryReportDto
{
    public Guid BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
}