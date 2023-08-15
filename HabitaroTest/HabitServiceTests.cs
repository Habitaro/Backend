using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models.Contracts;
using WebApi.Models.Services;
using WebApi.Models.Services.Abstractions;
using WebApi.Models.Services.Helpers;

namespace HabitaroTest
{
    [TestFixture]
    public class HabitServiceTests
    {
        private IHabitService _service;
        private HabitaroDbContext _context;

        [SetUp]
        public void SetUp()
        {
            var dbOptions = new DbContextOptionsBuilder<HabitaroDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var dbContext = new HabitaroDbContext(dbOptions);

            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<HabitaroMapProfile>()));

            foreach (var user in TestDataHelper.GetFakeUsersList())
            {
                dbContext.Users.Add(user);
            }

            foreach (var rank in TestDataHelper.GetRanks())
            {
                dbContext.Ranks.Add(rank);
            }

            foreach (var habit in TestDataHelper.GetFakeHabitsList())
            {
                dbContext.Habits.Add(habit);
            }

            dbContext.SaveChanges();

            _context = dbContext;
            _service = new HabitService(new RepositoryManager(dbContext), mapper);
        }

        [Test]
        public async Task GetByUserId_WhenCalled_ReturnsListOfDtosWithProgress()
        {
            //Arrange
            var expected = new List<HabitReadDto>()
            {
                new HabitReadDto()
                {
                    Id = 1,
                    UserId = 1,
                    Name = "Drink water",
                    Description = "Drink at least 2l of water every day",
                    Progress = new Dictionary<DateOnly, bool>()
                    {
                        [DateOnly.FromDateTime(DateTime.Now)] = false,
                        [DateOnly.FromDateTime(DateTime.Now.AddDays(1))] = false
                    }
                },
                new HabitReadDto()
                {
                    Id = 2,
                    UserId = 1,
                    Name = "15 min of sport",
                    Description = "",
                    Progress = new Dictionary<DateOnly, bool>()
                    {
                        [DateOnly.FromDateTime(DateTime.Now)] = false,
                        [DateOnly.FromDateTime(DateTime.Now.AddDays(1))] = false
                    }
                }
            };

            //Act
            var actual = await _service.GetByUserId(1);

            //Assert
            Assert.That(actual, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(actual.Count(), Is.EqualTo(expected.Count));
                Assert.That(actual.First().Progress, Is.Not.Null);
                Assert.That(actual.Last().Progress, Is.Not.Null);
            });
            Assert.Multiple(() =>
            {
                Assert.That(actual.First().Progress, Has.Count.EqualTo(expected.First().Progress.Count));
                Assert.That(actual.Last().Progress, Has.Count.EqualTo(expected.Last().Progress.Count));
            });
        }

        [Test]
        public async Task Add_WhenCalled_AddsHabitWithProgress()
        {
            //Arrange
            var expected = new HabitCreationDto()
            {
                UserId = 1,
                Name = "Test habit"
            };

            //Act
            await _service.Add(expected);
            var actualHabits = await _service.GetByUserId(expected.UserId);

            //Assert
            Assert.That(actualHabits, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(actualHabits.Last().Name, Is.EqualTo(expected.Name));
                Assert.That(actualHabits.Last().Progress, Is.Not.Null);
            });
            Assert.That(actualHabits.Last().Progress, Has.Count.EqualTo(2));
        }
    }
}
