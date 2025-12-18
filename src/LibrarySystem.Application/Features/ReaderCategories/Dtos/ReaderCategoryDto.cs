namespace LibrarySystem.Application.Features.ReaderCategories.Dtos;
public class ReaderCategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal DiscountPercentage { get; set; }
}