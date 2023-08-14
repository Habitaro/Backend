using AutoMapper;
using DataAccess.Repositories.Abstractions;
using WebApi.Models.Contracts;
using WebApi.Models.Services.Abstractions;

namespace WebApi.Models.Services
{
    public class HabitService : IHabitService
    {
        private readonly IRepositoryManager _manager;

        private readonly IMapper _mapper;

        public HabitService(IRepositoryManager manager, IMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }

        public Task Add(HabitCreationDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<HabitReadDto>> GetByUserId(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
