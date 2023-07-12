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

        User GetById(int id);

        void Update(User user);

        void DeleteById(int id);

        void Delete(User user);
    }
}
