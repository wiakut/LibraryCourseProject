namespace LibrarySystem.Application.Features.Reports.Dtos;
public class OverdueBooksReportDto
{
    public Guid RentalTransactionId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public string BookAuthor { get; set; } = string.Empty;
    public string ReaderName { get; set; } = string.Empty;
    public string ReaderPhone { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime ExpectedReturnDate { get; set; }
    public int OverdueDays { get; set; }
    public decimal EstimatedFine { get; set; }
}