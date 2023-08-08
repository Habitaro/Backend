using WebApi.Models.Contracts;

namespace WebApi.Models.Services.Abstractions
{
    public interface IUserService
    {
        IEnumerable<UserModel> GetAll();

        UserModel? GetById(int id);

        UserModel? GetByEmail(string email);

        void Create(UserForCreationDto dtoModel, string pepper);

        void Update(UserForEditDto dtoModel, int id);

        void Remove(UserModel model);

        void RemoveById(int id);

        bool VerifyPassword(UserModel model, string password, string pepper);

        void AddExp(UserModel model, int exp);
    }
}
