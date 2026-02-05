using LibrarySystem.Domain.Entities;
namespace LibrarySystem.Domain.Interfaces;
public interface IRentalPricingStrategy
{
    decimal CalculateRentalCost(Book book, DateTime issueDate, DateTime expectedReturnDate, ReaderCategory? category);
    decimal CalculateDiscount(Book book, ReaderCategory? category);
}