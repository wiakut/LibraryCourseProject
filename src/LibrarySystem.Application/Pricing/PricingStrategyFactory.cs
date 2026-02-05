using LibrarySystem.Application.Pricing.Strategies;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces;
namespace LibrarySystem.Application.Pricing;
public class PricingStrategyFactory
{
    private readonly StandardRentalPricingStrategy _standardStrategy;
    private readonly DiscountedRentalPricingStrategy _discountedStrategy;
    private readonly StandardFineCalculationStrategy _standardFineStrategy;
    private readonly VipFineCalculationStrategy _vipFineStrategy;
    
    public PricingStrategyFactory()
    {
        _standardStrategy = new StandardRentalPricingStrategy();
        _discountedStrategy = new DiscountedRentalPricingStrategy();
        _standardFineStrategy = new StandardFineCalculationStrategy();
        _vipFineStrategy = new VipFineCalculationStrategy();
    }
    
    public IRentalPricingStrategy GetPricingStrategy(ReaderCategory? category)
    {
        if (category != null && category.DiscountPercentage > 0)
        {
            return _discountedStrategy;
        }
        return _standardStrategy;
    }
    
    public IFineCalculationStrategy GetFineStrategy(ReaderCategory? category)
    {
        if (category != null && category.Name.Contains("VIP", StringComparison.OrdinalIgnoreCase))
        {
            return _vipFineStrategy;
        }
        return _standardFineStrategy;
    }
}