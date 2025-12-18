using LibrarySystem.Domain.Enums;
namespace LibrarySystem.Application.Features.Rentals.Dtos;
public class RentalTransactionDto
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public string? BookTitle { get; set; }
    public Guid ReaderId { get; set; }
    public string? ReaderName { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime ExpectedReturnDate { get; set; }
    public DateTime? ActualReturnDate { get; set; }
    public RentalStatus Status { get; set; }
    public decimal BaseRentalCost { get; set; }
    public decimal FinalRentalCost { get; set; }
    public decimal PledgeAmount { get; set; }
    public decimal RefundAmount { get; set; }
    public decimal FineAmount { get; set; }
}