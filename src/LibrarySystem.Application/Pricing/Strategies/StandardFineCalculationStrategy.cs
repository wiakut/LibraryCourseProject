using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces;

namespace LibrarySystem.Application.Pricing.Strategies;

public class StandardFineCalculationStrategy : IFineCalculationStrategy
{
    private const decimal DailyFineRate = 2.0m;

    public decimal CalculateFine(RentalTransaction transaction, DateTime actualReturnDate, decimal damageCost, ReaderCategory? category)
    {
        decimal overdueFine = 0;
        
        if (actualReturnDate > transaction.ExpectedReturnDate)
        {
            var overdueDays = (actualReturnDate - transaction.ExpectedReturnDate).Days;
            overdueFine = overdueDays * DailyFineRate;
        }
        
        return overdueFine + damageCost;
    }
}

