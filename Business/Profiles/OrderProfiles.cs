using AutoMapper;
using Core.Concretes.DTOs;
using Core.Concretes.Entities;

namespace Business.Profiles
{
    public class OrderProfiles : Profile
    {
        public OrderProfiles()
        {
            CreateMap<Order, OrderDto>();

            CreateMap<OrderItem, OrderItemDto>().ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductImage, opt => opt.MapFrom(src => src.Product.Images.FirstOrDefault(x => x.IsCoverImage)))
                .ForMember(dest => dest.ListPrice, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.DiscountValue, opt => opt.MapFrom(src => src.Product.Price * src.Product.DiscountRate / 100));
        }
    }
}
