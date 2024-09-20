using AutoMapper;
using WebShopAPI.Data;
using WebShopAPI.Models;

namespace WebShopAPI.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper() {
            CreateMap<Product, ProductModel>().ReverseMap();
        }
    }
}
