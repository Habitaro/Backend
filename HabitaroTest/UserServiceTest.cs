using AutoMapper;
using DataAccess.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Models.Contracts;
using WebApi.Models.Services;
using WebApi.Models.Services.Abstractions;

namespace HabitaroTest
{
    [TestFixture]
    public class UserServiceTest
    {
        private IUserService _service;
        
        [SetUp]
        public void SetUp() 
        {
            var dbOptions = new DbContextOptionsBuilder<HabitaroDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var dbContext = new HabitaroDbContext(dbOptions);

            var configurationData = new Dictionary<string, string>()
            {
                {"PasswordPepper", "Aboba123"},
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configurationData)
                .Build();

            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<HabitaroMapProfile>()));

            foreach (var user in TestDataHelper.GetFakeUsersList())
            {
                dbContext.Users.Add(user);
            }

            foreach (var rank in TestDataHelper.GetRanks())
            {
                dbContext.Ranks.Add(rank);
            }

            dbContext.SaveChanges();

            _service = new UserService(new RepositoryManager(dbContext), mapper);
        }

        [Test]
        public async Task GetAllAsDto_WhenCalled_ReturnsAllUsersDto()
        {
            //Arrange
            int expectedQuantity = TestDataHelper.GetFakeUsersList().Count;

            //Act
            var usersDto = await _service.GetAllAsDto();

            //Assert
            Assert.That(usersDto.Count(), Is.EqualTo(expectedQuantity));
        }

        [Test]
        public async Task GetByIdAsDto_WhenCalled_ReturnsCorrectDto()
        {
            //Arrange
            var expected = new UserReadDto()
            {
                Id = 1,
                UserName = "John Doe",
                Email = "johndoe@test.com",
                Status = "",
                AvatarId = 1,
                CurrentExp = 0,
                RequiredExp = 1000,
                RankId = 1,
                Rank = "Bronze cat 1",
            };

            //Act
            var actual = await _service.GetByIdAsDto(1);

            //Assert
            Assert.That(actual, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(actual.Id, Is.EqualTo(expected.Id));
                Assert.That(actual.Email, Is.EqualTo(expected.Email));
                Assert.That(actual.UserName, Is.EqualTo(expected.UserName));
            });
        }

        [Test]
        public async Task GetByEmailAsDto_WhenCalled_ReturnsCorrectDto()
        {
            //Arrange
            var expected = new UserReadDto()
            {
                Id = 1,
                UserName = "John Doe",
                Email = "johndoe@test.com",
                Status = "",
                AvatarId = 1,
                CurrentExp = 0,
                RequiredExp = 1000,
                RankId = 1,
                Rank = "Bronze cat 1",
            };

            //Act
            var actual = await _service.GetByEmailAsDto(expected.Email);

            //Assert
            Assert.That(actual, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(actual.Id, Is.EqualTo(expected.Id));
                Assert.That(actual.Email, Is.EqualTo(expected.Email));
                Assert.That(actual.UserName, Is.EqualTo(expected.UserName));
            });
        }

        [Test]
        public async Task GetAllAsModel_WhenCalled_ReturnsAlUserModels()
        {
            //Arrange
            int expectedQuantity = TestDataHelper.GetFakeUsersList().Count;

            //Act
            var userModels = await _service.GetAllAsModel();

            //Assert
            Assert.That(userModels.Count(), Is.EqualTo(expectedQuantity));
        }

        [Test]
        public async Task GetByIdAsModel_WhenCalled_ReturnsCorrectModel()
        {
            var entity = TestDataHelper.GetFakeUsersList()[0];

            var expected = new UserModel()
            {
                Id = entity.Id,
                UserName = entity.UserName,
                Email = entity.Email,
                PasswordHash = entity.PasswordHash,
                PasswordSalt = entity.PasswordSalt,
                Status = entity.Status,
                AvatarId = entity.AvatarId,
                CurrentExp = entity.CurrentExp,
                RequiredExp = entity.RequiredExp,
                RankId = entity.RankId,
                Rank = "Bronze cat 1",
            };

            //Act
            var actual = await _service.GetByIdAsModel(1);

            //Assert
            Assert.That(actual, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(actual.Id, Is.EqualTo(expected.Id));
                Assert.That(actual.Email, Is.EqualTo(expected.Email));
                Assert.That(actual.UserName, Is.EqualTo(expected.UserName));
            });
        }

        [Test]
        public async Task GetByEmailAsModel_WhenCalled_ReturnsCorrectModel()
        {
            var entity = TestDataHelper.GetFakeUsersList()[0];

            var expected = new UserModel()
            {
                Id = entity.Id,
                UserName = entity.UserName,
                Email = entity.Email,
                PasswordHash = entity.PasswordHash,
                PasswordSalt = entity.PasswordSalt,
                Status = entity.Status,
                AvatarId = entity.AvatarId,
                CurrentExp = entity.CurrentExp,
                RequiredExp = entity.RequiredExp,
                RankId = entity.RankId,
                Rank = "Bronze cat 1",
            };

            //Act
            var actual = await _service.GetByEmailAsModel(entity.Email);

            //Assert
            Assert.That(actual, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(actual.Id, Is.EqualTo(expected.Id));
                Assert.That(actual.Email, Is.EqualTo(expected.Email));
                Assert.That(actual.UserName, Is.EqualTo(expected.UserName));
            });
        }

        [Test]
        public async Task Create_WhenCalled_CreatesUserAndAddsToDb()
        {
            //Arrange
            var dto = new UserCreationDto()
            {
                Email = "user@test.example",
                Username = "Test user",
                Password = "Test12345",
                ConfirmPassword = "Test12345"
            };

            //Act
            await _service.Create(dto, TestDataHelper._pepper);
            var actual = await _service.GetByEmailAsModel(dto.Email);

            //Assert
            Assert.That(actual, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(actual.Email, Is.EqualTo(dto.Email));
                Assert.That(actual.UserName, Is.EqualTo(dto.Username));
                Assert.That(_service.VerifyPassword(actual, dto.Password, TestDataHelper._pepper), Is.True);
            });
        }

        [Test]
        public void Create_WhenCalledWithExistingEmail_ThrowsException()
        {
            //Arrange
            var dto = new UserCreationDto()
            {
                Email = "johndoe@test.com",
                Username = "Test user",
                Password = "Test12345",
                ConfirmPassword = "Test12345"
            };

            //Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.Create(dto, TestDataHelper._pepper));
        }


        [Test]
        public async Task Update_WhenCalled_UpdatesUsersData()
        {
            //Arrange
            var dto = new UserEditDto()
            {
                Username = "Test",
                AvatarId = 20,
                Status = "Test status"
            };

            //Act
            await _service.Update(dto, 1);
            var actual = await _service.GetByIdAsModel(1);

            //Assert
            Assert.That(actual, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(actual.UserName, Is.EqualTo(dto.Username));
                Assert.That(actual.AvatarId, Is.EqualTo(dto.AvatarId));
                Assert.That(actual.Status, Is.EqualTo(dto.Status));
            });
        }

        [Test]
        public async Task Remove_WhenCalled_RemovesUserFromDb()
        {
            //Arrange
            var entity = TestDataHelper.GetFakeUsersList()[0];

            var user = new UserModel()
            {
                Id = entity.Id,
                UserName = entity.UserName,
                Email = entity.Email,
                PasswordHash = entity.PasswordHash,
                PasswordSalt = entity.PasswordSalt,
                Status = entity.Status,
                AvatarId = entity.AvatarId,
                CurrentExp = entity.CurrentExp,
                RequiredExp = entity.RequiredExp,
                RankId = entity.RankId,
                Rank = "Bronze cat 1",
            };

            //Act
            await _service.Remove(user);

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.GetByIdAsDto(1));
        }

        [Test]
        public async Task RemoveById_WhenCalled_RemovesUserFromDb()
        {
            //Arrange

            //Act
            await _service.RemoveById(1);

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.RemoveById(1));
        }

        [Test]
        public async Task AddExp_WhenCalled_AddsExp()
        {
            //Arrange
            int expectedExp = 100;

            //Act
            await _service.AddExp(1, expectedExp);
            var user = await _service.GetByIdAsDto(1);

            //Assert
            Assert.That(user.CurrentExp, Is.EqualTo(expectedExp));
        }

        [Test]
        public async Task AddExp_WhenExpMoreThanRequired_ChangesRank()
        {
            //Arrange
            int expToAdd = 1200;
            int expectedRank = 2;
            int expectedExp = 200;
            int expectedRequiredExp = 1500;

            //Act
            await _service.AddExp(1, expToAdd);
            var user = await _service.GetByIdAsDto(1);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(user.CurrentExp, Is.EqualTo(expectedExp));
                Assert.That(user.RankId, Is.EqualTo(expectedRank));
                Assert.That(user.RequiredExp, Is.EqualTo(expectedRequiredExp));
            });
        }
    }
}
