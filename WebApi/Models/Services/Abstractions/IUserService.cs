using WebApi.Models.Contracts;

namespace WebApi.Models.Services.Abstractions
{
    public interface IUserService
    {
        IEnumerable<UserModel> GetAll();

        UserModel? GetById(int id);

        UserModel? GetByEmail(string email);

        void Create(UserForCreationDto dtoModel);

        void Update(UserModel model);

        void Remove(UserModel model);
        bool ComparePassword(UserModel user, string password);
    }
}
