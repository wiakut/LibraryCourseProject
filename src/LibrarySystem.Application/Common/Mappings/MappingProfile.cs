using AutoMapper;
using LibrarySystem.Application.Features.Books.Dtos;
using LibrarySystem.Application.Features.Readers.Dtos;
using LibrarySystem.Application.Features.ReaderCategories.Dtos;
using LibrarySystem.Application.Features.RentalRequests.Dtos;
using LibrarySystem.Application.Features.Rentals.Dtos;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Book, BookDto>();

        CreateMap<Reader, ReaderDto>();

        CreateMap<ReaderCategory, ReaderCategoryDto>();

        CreateMap<RentalTransaction, RentalTransactionDto>()
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book != null ? src.Book.Title : null))
            .ForMember(dest => dest.ReaderName, opt => opt.MapFrom(src => src.Reader != null ? src.Reader.Name : null));

        CreateMap<RentalRequest, RentalRequestDto>()
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book != null ? src.Book.Title : null))
            .ForMember(dest => dest.ReaderName, opt => opt.MapFrom(src => src.Reader != null ? src.Reader.Name : null));
    }
}

