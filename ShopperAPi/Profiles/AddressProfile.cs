using AutoMapper;
using Core.Entities;
using ShopperAPi.DTOS.OrderDto;

namespace ShopperAPi.Profiles
{
    public class AddressProfile:Profile
    {
        public AddressProfile()
        {
            CreateMap<AddressDto, Address>();
        }
    }
}
