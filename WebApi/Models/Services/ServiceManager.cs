using AutoMapper;
using DataAccess;
using DataAccess.Repositories.Abstractions;
using WebApi.Models.Services.Abstractions;

namespace WebApi.Models.Services
{
    public class ServiceManager : IServiceManager
    {
        public IUserService UserService { get; }

        public ServiceManager(IRepositoryManager manager, IMapper mapper)
        {
            UserService = new UserService(manager, mapper);
        }
    }
}
