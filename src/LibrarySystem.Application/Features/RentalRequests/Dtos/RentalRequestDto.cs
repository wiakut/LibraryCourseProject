using LibrarySystem.Domain.Enums;
namespace LibrarySystem.Application.Features.RentalRequests.Dtos;
public class RentalRequestDto
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public Guid ReaderId { get; set; }
    public string ReaderName { get; set; } = string.Empty;
    public DateTime RequestDate { get; set; }
    public RentalRequestStatus Status { get; set; }
    public DateTime? ExpectedReturnDate { get; set; }
    public string? DenialReason { get; set; }
    public DateTime? ProcessedDate { get; set; }
    public string? ProcessedByUserId { get; set; }
}