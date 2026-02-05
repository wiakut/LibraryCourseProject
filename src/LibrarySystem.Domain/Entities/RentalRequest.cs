using LibrarySystem.Domain.Enums;
namespace LibrarySystem.Domain.Entities;
public class RentalRequest
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public Guid ReaderId { get; set; }
    public DateTime RequestDate { get; set; }
    public RentalRequestStatus Status { get; set; }
    public DateTime? ExpectedReturnDate { get; set; }
    public string? DenialReason { get; set; }
    public DateTime? ProcessedDate { get; set; }
    public string? ProcessedByUserId { get; set; }
    public Book Book { get; set; } = null!;
    public Reader Reader { get; set; } = null!;
}