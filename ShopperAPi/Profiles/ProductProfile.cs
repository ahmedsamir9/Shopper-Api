using AutoMapper;
using Core.Entities;
using ShopperAPi.DTOS;
using ShopperAPi.DTOS.ProductDTOs;
using ShopperAPi.Helpers;

namespace ShopperAPi.Profiles
{
    public class ProductProfile:Profile
    {
        public ProductProfile() {
            CreateMap<Core.Entities.Product, ProductDto>()
                    .ForMember(p => p.CategoryName, o => o.MapFrom(p => p.Category.Name));
            CreateMap<ProductMainpulationsDto, Product>()
                .ForMember(p => p.ImagePath, o => o.MapFrom<ImageResovler>());
        }
      
    }


   
}
