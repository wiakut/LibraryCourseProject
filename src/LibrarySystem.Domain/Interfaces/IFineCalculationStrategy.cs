using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces;

public interface IFineCalculationStrategy
{
    decimal CalculateFine(RentalTransaction transaction, DateTime actualReturnDate, decimal damageCost, ReaderCategory? category);
}

