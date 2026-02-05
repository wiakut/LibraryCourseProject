using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces;
namespace LibrarySystem.Application.Pricing.Strategies;
public class DiscountedRentalPricingStrategy : IRentalPricingStrategy
{
    private readonly StandardRentalPricingStrategy _baseStrategy;
    public DiscountedRentalPricingStrategy()
    {
        _baseStrategy = new StandardRentalPricingStrategy();
    }
    public decimal CalculateRentalCost(Book book, DateTime issueDate, DateTime expectedReturnDate, ReaderCategory? category)
    {
        return _baseStrategy.CalculateRentalCost(book, issueDate, expectedReturnDate, category);
    }
    public decimal CalculateDiscount(Book book, ReaderCategory? category)
    {
        if (category == null || category.DiscountPercentage <= 0)
        {
            return 0;
        }
        return category.DiscountPercentage;
    }
}