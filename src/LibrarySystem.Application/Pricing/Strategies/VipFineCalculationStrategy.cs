using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces;

namespace LibrarySystem.Application.Pricing.Strategies;

public class VipFineCalculationStrategy : IFineCalculationStrategy
{
    private const decimal DailyFineRate = 2.0m;
    private const int GracePeriodDays = 5;
    private const decimal DamageCostDiscount = 0.20m;

    public decimal CalculateFine(RentalTransaction transaction, DateTime actualReturnDate, decimal damageCost, ReaderCategory? category)
    {
        decimal overdueFine = 0;
        
        if (actualReturnDate > transaction.ExpectedReturnDate)
        {
            var overdueDays = (actualReturnDate - transaction.ExpectedReturnDate).Days;
            
            var chargeableDays = Math.Max(0, overdueDays - GracePeriodDays);
            overdueFine = chargeableDays * DailyFineRate;
        }
        
        var discountedDamageCost = damageCost * (1 - DamageCostDiscount);
        
        return overdueFine + discountedDamageCost;
    }
}

