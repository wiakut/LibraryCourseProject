namespace LibrarySystem.Domain.Entities;

public class Reader
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public Guid ReaderCategoryId { get; set; }
    public string? UserId { get; set; } // Optional link to Identity User

    // Navigation properties
    public ReaderCategory ReaderCategory { get; set; } = null!;
    public ICollection<RentalTransaction> RentalTransactions { get; set; } = new List<RentalTransaction>();
}


