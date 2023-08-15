using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models.Services;
using WebApi.Models.Services.Helpers;

namespace HabitaroTest
{
    [TestFixture]
    public class HabitRepositoryTest
    {
        private IRepositoryManager _manager;
        private HabitaroDbContext _context;

        [SetUp]
        public void Setup()
        {
            var dbOptions = new DbContextOptionsBuilder<HabitaroDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new HabitaroDbContext(dbOptions);

            foreach (var user in TestDataHelper.GetFakeUsersList())
            {
                _context.Users.Add(user);
            }

            foreach (var rank in TestDataHelper.GetRanks())
            {
                _context.Ranks.Add(rank);
            }

            foreach (var habit in TestDataHelper.GetFakeHabitsList())
            {
                _context.Habits.Add(habit);
            }

            _context.SaveChanges();

            _manager = new RepositoryManager(_context);
        }

        [Test]
        public async Task Add_WhenCalled_AddsHabitWhitHabitDays()
        {
            //Arrange
            var expected = new Habit()
            {
                UserId = 2,
                Name = "Test habit",
                Description = "Test description",
                Progress = new List<HabitDay>()
                {
                    new HabitDay()
                    {
                        Date = DateTime.Now,
                        IsCompleted = false,
                    },
                    new HabitDay()
                    {
                        Date = DateTime.Now.AddDays(1),
                        IsCompleted = false
                    }
                }
            };

            //Act
            await _manager.HabitRepository.Add(expected);
            await _manager.SaveChanges();
            var actual = _context.Habits.Include(h => h.Progress).Last();

            //Assert
            Assert.That(actual, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(actual.Name, Is.EqualTo(expected.Name));
                Assert.That(actual.Progress, Is.Not.Null);
                Assert.That(actual.Progress.Count(), Is.EqualTo(expected.Progress.Count()));
            });
        }

        [Test]
        public async Task GetByUserId_WhenCalled_ReturnsListOfHabitsWithProgress()
        {
            //Arrange
            var expected = TestDataHelper.GetFakeHabitsList().Where(h => h.UserId == 1);

            //Act
            var actual = await _manager.HabitRepository.GetByUserId(1);

            //Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Count(), Is.EqualTo(expected.Count()));
        }
    }
}
