namespace WebApi.Models.Services.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        IUserService UserService { get; }

        IHabitService HabitService { get; }
    }
}
