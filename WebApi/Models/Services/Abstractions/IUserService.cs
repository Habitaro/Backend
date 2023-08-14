using WebApi.Models.Contracts;

namespace WebApi.Models.Services.Abstractions
{
    public interface IUserService
    {
        Task<IEnumerable<UserReadDto>> GetAllAsDto();

        Task<UserReadDto> GetByIdAsDto(int id);

        Task<UserReadDto> GetByEmailAsDto(string email);

        Task<IEnumerable<UserModel>> GetAllAsModel();

        Task<UserModel> GetByIdAsModel(int id);

        Task<UserModel> GetByEmailAsModel(string email);

        Task Create(UserCreationDto dtoModel);

        Task Update(UserEditDto dtoModel, int id);

        Task Remove(UserModel model);

        Task RemoveById(int id);

        bool VerifyPassword(UserModel model, string password);

        Task AddExp(int id, int exp);
    }
}
