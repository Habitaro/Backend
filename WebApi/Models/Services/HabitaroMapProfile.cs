using AutoMapper;
using DataAccess.Entities;
using WebApi.Models.Contracts;

namespace WebApi.Models.Services
{
    public class HabitaroMapProfile : Profile
    {
        public HabitaroMapProfile() 
        {
            CreateMap<User, UserModel>()
                .ForMember(um => um.Rank, u => u.MapFrom(u => u.Rank.Name));

            CreateMap<UserModel, User>()
                .ForMember(u => u.Rank, um => um.Ignore());

            CreateMap<User, UserReadDto>()
                .ForMember(dto => dto.Rank, u => u.MapFrom(u => u.Rank.Name));
        }
    }
}
