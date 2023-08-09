using AutoMapper;
using DataAccess;
using DataAccess.Repositories.Abstractions;
using WebApi.Models.Services.Abstractions;

namespace WebApi.Models.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        public IUserService UserService { get; }

        public UnitOfWork(IRepositoryManager manager, IMapper mapper)
        {
            UserService = new UserService(manager, mapper);
        }
    }
}
