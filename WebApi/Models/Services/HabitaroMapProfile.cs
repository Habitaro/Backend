using AutoMapper;
using DataAccess.Entities;

namespace WebApi.Models.Services
{
    public class HabitaroMapProfile : Profile
    {
        public HabitaroMapProfile() 
        {
            CreateMap<User, UserModel>()
                .ForMember(um => um.Rank, u => u.MapFrom(u => u.Rank.Name));
        }
    }
}
