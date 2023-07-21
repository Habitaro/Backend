using WebApi.Models.Contracts;

namespace WebApi.Models.Services.Abstractions
{
    public interface IUserService
    {
        IEnumerable<UserModel> GetAll();

        UserModel? GetById(int id);

        UserModel? GetByEmail(string email);

        void Create(UserForCreationDto dtoModel);

        void Update(UserForEditDto dtoModel, int id);

        void Remove(UserModel model);

        void RemoveById(int id);

        bool VerifyPassword(UserModel user, string password);
    }
}
