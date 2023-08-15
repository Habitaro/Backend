using MockQueryable.Moq;
using WebApi.Models.Contracts;
using WebApi.Models.Services.Helpers;

namespace HabitaroTest
{
    [TestFixture]
    public class UserRepositoryTest
    {
        private IRepositoryManager _manager;

        [SetUp]
        public void Setup()
        {
            var mock = new Mock<HabitaroDbContext>();
            var fakeUsers = TestDataHelper.GetFakeUsersList();

            mock.Setup<DbSet<User>>(m => m.Users)
                .Returns(TestDataHelper.GetFakeDbSet(fakeUsers));

            _manager = new RepositoryManager(mock.Object);
        }

        [Test]
        public async Task GetAll_WhenCalled_ReturnsUsers()
        {
            //Arrange
            var referenceUsers = TestDataHelper.GetFakeUsersList();
            
            //Act
            var users = await _manager.UserRepository.GetAll();

            //Assert
            Assert.That(users.Count(), Is.EqualTo(referenceUsers.Count));
        }

        [Test]
        public async Task GetById_WhenCalled_ReturnsUser()
        {
            //Arrange
            var referanceUser = TestDataHelper.GetFakeUsersList()[1];

            //Act
            var user = await _manager.UserRepository.GetById(2);

            //Assert
            Assert.That(user, Is.Not.Null);
            Assert.That(user.Id, Is.EqualTo(referanceUser.Id));
        }

        [Test]
        public async Task GetByEmail_WhenCalled_ReturnsUser()
        {
            //Arrange
            var referanceUser = TestDataHelper.GetFakeUsersList()[1];

            //Act
            var user = await _manager.UserRepository.GetByEmail("mark.lu@test.com");

            //Assert
            Assert.That(user, Is.Not.Null);
            Assert.That(user.Id, Is.EqualTo(referanceUser.Id));
        }

        [Test]
        public async Task Add_WhenCalled_AddsUser()
        {
            //Arrange
            var passwordSalt = HashHelper.GenerateSalt();
            User user = new()
            {
                UserName = "Test Name",
                Email = "email@test.com",
                RankId = 1,
                PasswordHash = HashHelper.ComputeHash("Test123", passwordSalt, "Aboba123", 3),
                PasswordSalt = passwordSalt,
            };

            //Act
            await _manager.UserRepository.Add(user);
            await _manager.SaveChanges();
            var addedUser = await _manager.UserRepository.GetByEmail(user.Email);

            //Assert
            Assert.That(addedUser, Is.Not.Null);
            Assert.That(addedUser.Email, Is.EqualTo(user.Email));
        }
    }
}