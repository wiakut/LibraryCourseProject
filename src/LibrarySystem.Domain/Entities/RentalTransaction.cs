using LibrarySystem.Domain.Enums;

namespace LibrarySystem.Domain.Entities;

public class RentalTransaction
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public Guid ReaderId { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime ExpectedReturnDate { get; set; }
    public DateTime? ActualReturnDate { get; set; }
    public RentalStatus Status { get; set; }
    public decimal BaseRentalCost { get; set; }
    public decimal FinalRentalCost { get; set; }
    public decimal PledgeAmount { get; set; }
    public decimal RefundAmount { get; set; }
    public decimal FineAmount { get; set; }

    // Navigation properties
    public Book Book { get; set; } = null!;
    public Reader Reader { get; set; } = null!;
    public ICollection<Fine> Fines { get; set; } = new List<Fine>();
}


