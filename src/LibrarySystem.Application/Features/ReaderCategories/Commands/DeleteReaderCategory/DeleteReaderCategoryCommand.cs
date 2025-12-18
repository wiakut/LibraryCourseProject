using MediatR;
namespace LibrarySystem.Application.Features.ReaderCategories.Commands.DeleteReaderCategory;
public record DeleteReaderCategoryCommand(Guid Id) : IRequest<Unit>;