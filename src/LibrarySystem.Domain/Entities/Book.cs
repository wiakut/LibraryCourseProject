namespace LibrarySystem.Domain.Entities;

public class Book
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public decimal PledgeValue { get; set; }
    public decimal BaseRentalCost { get; set; }
    public int TotalCount { get; set; }
    public int AvailableCount { get; set; }
    public int InRentCount { get; set; }

    public ICollection<RentalTransaction> RentalTransactions { get; set; } = new List<RentalTransaction>();
    public ICollection<RentalRequest> RentalRequests { get; set; } = new List<RentalRequest>();
}


