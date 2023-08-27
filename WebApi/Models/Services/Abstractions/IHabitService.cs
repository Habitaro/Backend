using WebApi.Models.Contracts;

namespace WebApi.Models.Services.Abstractions
{
    public interface IHabitService
    {
        Task Add(HabitCreationDto dto, int userId);

        Task<HabitReadDto> GetById(int id);

        Task<IEnumerable<HabitReadDto>> GetByUserId(int userId);
        Task<IEnumerable<HabitReadDto>> GetByUserIdDesc(int userId);
        Task<IEnumerable<HabitReadDto>> GetSortedByNameAsc(int userId);
        Task<IEnumerable<HabitReadDto>> GetSortedByNameDesc(int userId);
        Task Update(int id, HabitEditDto dto);

        Task Delete(int id);
    }
}
