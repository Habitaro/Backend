namespace WebApi.Models.Services.Abstractions
{
    public interface IUnitOfWork
    {
        IUserService UserService { get; }
    }
}
