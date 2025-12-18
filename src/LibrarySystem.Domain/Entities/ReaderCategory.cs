namespace LibrarySystem.Domain.Entities;

public class ReaderCategory
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal DiscountPercentage { get; set; }

    // Navigation property
    public ICollection<Reader> Readers { get; set; } = new List<Reader>();
}


