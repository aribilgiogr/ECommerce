using AutoMapper;
using Core.Concretes.DTOs;
using Core.Concretes.Entities;

namespace Business.Profiles
{
    public class AuthProfiles : Profile
    {
        public AuthProfiles()
        {
            // RegisterDto -> Customer mapping
            CreateMap<RegisterDto, Customer>();
        }
    }
}
