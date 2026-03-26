using AutoMapper;
using Core.Concretes.DTOs;
using Core.Concretes.Entities;

namespace Business.Profiles
{
    public class ShowroomProfiles : Profile
    {
        public ShowroomProfiles()
        {
            // src: Product, dest: ProductListItemDto
            CreateMap<Product, ProductListItemDto>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.SubcategoryName, opt => opt.MapFrom(src => src.Subcategory.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Subcategory.Category.Name))
                .ForMember(dest => dest.ReviewCount, opt => opt.MapFrom(src => src.Reviews.Count()))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Reviews.Any() ? src.Reviews.Average(x => x.Vote) : 0))
                .ForMember(dest => dest.CoverImageUrl, opt => opt.MapFrom(src => src.Images.Any() ? src.Images.FirstOrDefault(x => x.IsCoverImage).ImageUrl : string.Empty));
        }
    }
}
