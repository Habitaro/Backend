using WebApi.Models.Contracts;

namespace WebApi.Models.Services.Abstractions
{
    public interface IUserService
    {
        IEnumerable<UserReadDto> GetAllAsDto();

        UserReadDto? GetByIdAsDto(int id);

        UserReadDto? GetByEmailAsDto(string email);

        IEnumerable<UserModel> GetAllAsModel();

        UserModel? GetByIdAsModel(int id);

        UserModel? GetByEmailAsModel(string email);

        void Create(UserCreationDto dtoModel, string pepper);

        void Update(UserEditDto dtoModel, int id);

        void Remove(UserModel model);

        void RemoveById(int id);

        bool VerifyPassword(UserModel model, string password, string pepper);

        void AddExp(UserModel model, int exp);
    }
}
