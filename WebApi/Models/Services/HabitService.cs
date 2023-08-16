using AutoMapper;
using DataAccess.Entities;
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

        public async Task Add(HabitCreationDto dto, int userId)
        {
            var habit = _mapper.Map<Habit>(dto);
            habit.UserId = userId;
            habit.Progress = new List<HabitDay>()
            {
                new HabitDay()
                {
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    IsCompleted = false
                },
                new HabitDay()
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                    IsCompleted = false
                }
            };

            await _manager.HabitRepository.Add(habit);
            await _manager.SaveChanges();
        }

        public async Task<IEnumerable<HabitReadDto>> GetByUserId(int userId)
        {
            var habits = await _manager.HabitRepository.GetByUserId(userId);
            var dtos = _mapper.Map<IEnumerable<Habit>, IEnumerable<HabitReadDto>>(habits);

            return dtos;
        }

        public async Task<IEnumerable<HabitReadDto>> GetByUserIdDesc(int userId)
        {
            var habits = await _manager.HabitRepository.GetByUserId(userId);
            var dtos = _mapper.Map<IEnumerable<Habit>, IEnumerable<HabitReadDto>>(habits);

            return dtos.OrderByDescending(h => h.Id);
        }

        public async Task<IEnumerable<HabitReadDto>> GetSortedByNameAsc(int userId)
        {
            var habits = await _manager.HabitRepository.GetByUserId(userId);
            var dtos = _mapper.Map<IEnumerable<HabitReadDto>>(habits);

            return dtos.OrderBy(h => h.Name);
        }

        public async Task<IEnumerable<HabitReadDto>> GetSortedByNameDesc(int userId)
        {
            var habits = await _manager.HabitRepository.GetByUserId(userId);
            var dtos = _mapper.Map<IEnumerable<HabitReadDto>>(habits);

            return dtos.OrderByDescending(h => h.Name);
        }
    }
}
