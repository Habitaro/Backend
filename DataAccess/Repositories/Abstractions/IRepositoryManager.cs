using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Abstractions
{
    public interface IRepositoryManager : IDisposable
    {
        IUserRepository UserRepository { get; }

        IHabitRepository HabitRepository { get; }

        Task SaveChanges();
    }
}
