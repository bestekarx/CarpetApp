using AutoMapper;
using WebCarpetApp.Books;

namespace WebCarpetApp.Blazor.Client;

public class WebCarpetAppBlazorAutoMapperProfile : Profile
{
    public WebCarpetAppBlazorAutoMapperProfile()
    {
        CreateMap<BookDto, CreateUpdateBookDto>();
        
        //Define your AutoMapper configuration here for the Blazor project.
    }
}