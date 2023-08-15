using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models.Services.Helpers;

namespace HabitaroTest
{
    internal static class TestDataHelper
    {
        private const string _pepper = "Aboba123";

        private const int _iterations = 3;

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
                    RankId = 1
                },
                new User()
                {
                    Id = 2,
                    UserName = "Mark Luther",
                    Email = "mark.lu@test.com",
                    PasswordHash = HashHelper.ComputeHash("Password123", passwordSalt, _pepper, _iterations),
                    PasswordSalt = passwordSalt,
                    RankId = 1
                },
                new User()
                {
                    Id = 3,
                    UserName = "Renate Muller",
                    Email = "muller@test.com",
                    PasswordHash = HashHelper.ComputeHash("Password123", passwordSalt, _pepper, _iterations),
                    PasswordSalt = passwordSalt,
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

        public static List<Habit> GetFakeHabitsList()
        {
            var habits = new List<Habit>()
            {
                new Habit()
                {
                    Id = 1,
                    UserId = 1,
                    Name = "Drink water",
                    Description = "Drink at least 2l of water every day",
                    Progress = new List<HabitDay>()
                    {
                        new HabitDay()
                        {
                            Id = 1,
                            HabitId = 1,
                            Date = DateTime.Now,
                            IsCompleted = false
                        },
                        new HabitDay()
                        {
                            Id = 2,
                            HabitId = 1,
                            Date = DateTime.Now.AddDays(1),
                            IsCompleted = false
                        }
                    },
                },
                new Habit()
                {
                    Id = 2,
                    UserId = 1,
                    Name = "15 min of sport",
                    Description = "",
                    Progress = new List<HabitDay>()
                    {
                        new HabitDay()
                        {
                            Id = 3,
                            HabitId = 2,
                            Date = DateTime.Now,
                            IsCompleted = true,
                        },
                        new HabitDay()
                        {
                            Id = 4,
                            HabitId = 2,
                            Date = DateTime.Now.AddDays(1),
                            IsCompleted = false
                        }
                    }
                }
            };

            return habits;
        }
    }
}
