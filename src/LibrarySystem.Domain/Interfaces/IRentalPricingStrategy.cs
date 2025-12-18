using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces;

public interface IRentalPricingStrategy
{
    /// <summary>
    /// Calculates the base rental cost based on book's base rental cost and rental period
    /// </summary>
    /// <param name="book">The book being rented</param>
    /// <param name="issueDate">Date when the book is issued</param>
    /// <param name="expectedReturnDate">Expected return date</param>
    /// <param name="category">Reader category (for potential discount calculation)</param>
    /// <returns>Base rental cost</returns>
    decimal CalculateRentalCost(Book book, DateTime issueDate, DateTime expectedReturnDate, ReaderCategory? category);

    /// <summary>
    /// Calculates the discount amount based on reader category
    /// </summary>
    /// <param name="book">The book being rented</param>
    /// <param name="category">Reader category with discount percentage</param>
    /// <returns>Discount amount</returns>
    decimal CalculateDiscount(Book book, ReaderCategory? category);

    /// <summary>
    /// Calculates fine amount for overdue returns and damage
    /// </summary>
    /// <param name="transaction">The rental transaction</param>
    /// <param name="actualReturnDate">Actual date when book is returned</param>
    /// <param name="damageCost">Cost of any damage to the book</param>
    /// <returns>Total fine amount (overdue + damage)</returns>
    decimal CalculateFine(RentalTransaction transaction, DateTime actualReturnDate, decimal damageCost);
}


