namespace LibrarySystem.Domain.Entities;
public class Fine
{
    public Guid Id { get; set; }
    public Guid RentalTransactionId { get; set; }
    public decimal Amount { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public RentalTransaction RentalTransaction { get; set; } = null!;
}