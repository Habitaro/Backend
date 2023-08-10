using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models.Services;

namespace HabitaroTest
{
    internal static class TestDataHelper
    {
        public const string _pepper = "Aboba123";

        public const int _iterations = 3;

        public static DbSet<T> GetFakeDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));

            return dbSet.Object;
        }

        public static List<User> GetFakeUsersList()
        {
            var passwordSalt = HashHelper.GenerateSalt();

            return new List<User>()
            {
                new User()
                {
                    Id = 1,
                    UserName = "John Doe",
                    Email = "johndoe@test.com",
                    PasswordHash = HashHelper.ComputeHash("JohnDoe123", passwordSalt, _pepper, _iterations),
                    PasswordSalt = passwordSalt,
                    Status = "",
                    AvatarId = 1,
                    CurrentExp = 0,
                    RequiredExp = 1000,
                    RankId = 1
                },
                new User()
                {
                    Id = 2,
                    UserName = "Mark Luther",
                    Email = "mark.lu@test.com",
                    PasswordHash = HashHelper.ComputeHash("Password123", passwordSalt, _pepper, _iterations),
                    PasswordSalt = passwordSalt,
                    Status = "",
                    AvatarId = 1,
                    CurrentExp = 0,
                    RequiredExp = 1000,
                    RankId = 1
                },
                new User()
                {
                    Id = 3,
                    UserName = "Renate Muller",
                    Email = "muller@test.com",
                    PasswordHash = HashHelper.ComputeHash("Password123", passwordSalt, _pepper, _iterations),
                    PasswordSalt = passwordSalt,
                    Status = "",
                    AvatarId = 1,
                    CurrentExp = 0,
                    RequiredExp = 1000,
                    RankId = 1
                },
            };
        }

        public static List<Rank> GetRanks()
        {
            var ranks = new List<Rank>()
            {
                new Rank()
                {
                    Id = 1,
                    Name = "Bronze cat 1",
                }, new Rank()
                {
                    Id = 2,
                    Name = "Bronze cat 2",
                }, new Rank()
                {
                    Id = 3,
                    Name = "Bronze cat 3",
                }, new Rank()
                {
                    Id = 4,
                    Name = "Silver cat 1",
                }, new Rank()
                {
                    Id = 5,
                    Name = "Silver cat 2",
                }, new Rank()
                {
                    Id = 6,
                    Name = "Silver cat 3",
                }, new Rank()
                {
                    Id = 7,
                    Name = "Gold cat 1",
                }, new Rank()
                {
                    Id = 8,
                    Name = "Gold cat 2",
                }, new Rank()
                {
                    Id = 9,
                    Name = "Gold cat 3"
                }
            };

            return ranks;
        }
    }
}
