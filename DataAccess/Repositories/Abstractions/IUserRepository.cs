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
        IEnumerable<User> GetAll();

        User? GetById(int id);

        void Add(User user);

        void Update(User user);

        void Delete(User user);
    }
}
