namespace WebApi.Models.Services.Abstractions
{
    public interface IUserService
    {
        IEnumerable<UserModel> GetAll();

        UserModel? GetById(int id);

        UserModel? GetByEmail(string email);

        void Create(UserModel model);

        void Update(UserModel model);
    }
}
