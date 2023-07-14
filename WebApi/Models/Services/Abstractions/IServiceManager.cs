namespace WebApi.Models.Services.Abstractions
{
    public interface IServiceManager
    {
        IUserService UserService { get; }
    }
}
