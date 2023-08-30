using AutoMapper;
using DataAccess;
using DataAccess.Repositories.Abstractions;
using WebApi.Models.Services.Abstractions;

namespace WebApi.Models.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IRepositoryManager manager;

        private bool _isDisposed;

        public IUserService UserService { get; }

        public IHabitService HabitService { get; }

        public UnitOfWork(IRepositoryManager manager, IMapper mapper, IConfiguration configuration)
        {
            this.manager = manager;
            UserService = new UserService(manager, mapper, configuration);
            HabitService = new HabitService(manager, mapper);
        }

        public void Dispose()
        {
            if(_isDisposed) return;

            manager.Dispose();

            _isDisposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
