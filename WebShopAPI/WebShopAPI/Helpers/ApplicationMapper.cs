using AutoMapper;
using WebShopAPI.Data;
using WebShopAPI.Dtos;
using WebShopAPI.Models;

namespace WebShopAPI.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper() {
            CreateMap<Product, ProductModel>().ReverseMap();
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ImgPros))
                .ForMember(dest => dest.ProductItems, opt => opt.MapFrom(src => src.ProductItems));
            CreateMap<ProductItem, ProductItemDto>();
            CreateMap<ImgPro, ImgProDto>();
        }
    }
}
