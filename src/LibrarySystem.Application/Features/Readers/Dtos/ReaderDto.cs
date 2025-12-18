namespace LibrarySystem.Application.Features.Readers.Dtos;
public class ReaderDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public Guid ReaderCategoryId { get; set; }
    public string? UserId { get; set; }
}