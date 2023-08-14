using AutoMapper;
using DataAccess;
using DataAccess.Repositories.Abstractions;
using WebApi.Models.Services.Abstractions;

namespace WebApi.Models.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        public IUserService UserService { get; }

        public IHabitService HabitService { get; }

        public UnitOfWork(IRepositoryManager manager, IMapper mapper, IConfiguration configuration)
        {
            UserService = new UserService(manager, mapper, configuration);
            HabitService = new HabitService(manager, mapper);
        }
    }
}
