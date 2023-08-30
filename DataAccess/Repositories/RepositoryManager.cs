using DataAccess.Entities;
using DataAccess.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly HabitaroDbContext context;

        private bool _isDisposed;

        private IUserRepository userRepository;

        private IHabitRepository habitRepository;

        public IUserRepository UserRepository { get => userRepository; }

        public IHabitRepository HabitRepository { get => habitRepository; }

        public RepositoryManager(HabitaroDbContext context)
        {
            this.context = context;
            userRepository = new UserRepository(context);
            habitRepository = new HabitRepository(context);
        }

        public async Task SaveChanges()
        {
            await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (_isDisposed) return;

            context.Dispose();

            _isDisposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
