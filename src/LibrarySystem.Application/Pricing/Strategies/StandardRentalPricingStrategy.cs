using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces;
namespace LibrarySystem.Application.Pricing.Strategies;
public class StandardRentalPricingStrategy : IRentalPricingStrategy
{
    public decimal CalculateRentalCost(Book book, DateTime issueDate, DateTime expectedReturnDate, ReaderCategory? category)
    {
        var rentalPeriod = (expectedReturnDate - issueDate).Days;
        if (rentalPeriod < 1)
        {
            rentalPeriod = 1; // Minimum 1 day
        }
        return book.BaseRentalCost * rentalPeriod;
    }
    public decimal CalculateDiscount(Book book, ReaderCategory? category)
    {
        if (category == null || category.DiscountPercentage <= 0)
        {
            return 0;
        }
        return 0;
    }
}