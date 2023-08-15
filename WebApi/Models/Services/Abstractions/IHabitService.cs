using WebApi.Models.Contracts;

namespace WebApi.Models.Services.Abstractions
{
    public interface IHabitService
    {
        Task Add(HabitCreationDto dto);

        Task<IEnumerable<HabitReadDto>> GetByUserId(int userId);
    }
}
