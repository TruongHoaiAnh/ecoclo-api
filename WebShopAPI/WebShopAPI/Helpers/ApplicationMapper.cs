﻿using AutoMapper;
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
                .ForMember(dest => dest.ProductItems, opt => opt.MapFrom(src => src.ProductItems))
                .ReverseMap();
            CreateMap<ProductItem, ProductItemDto>().ReverseMap();
            CreateMap<ImgPro, ImgProDto>().ReverseMap();
            CreateMap<Review, ReviewDto>().ReverseMap();
            CreateMap<Order, OrderDto>()
				.ForMember(dest => dest.IdOrder, opt => opt.MapFrom(src => src.IdOrder))
                .ReverseMap();
			CreateMap<OrderDetail, OrderDetailDto>().ReverseMap()
				.ForMember(dest => dest.IdProItem, opt => opt.MapFrom(src => src.IdProItem))
				.ReverseMap();
            

		}
    }
}
