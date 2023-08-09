using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Abstractions
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();

        Task<User?> GetById(int id);

        Task<User?> GetByEmail(string email);

        Task Add(User user);

        void Update(User user);

        void Delete(User user);
    }
}
