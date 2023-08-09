using MockQueryable.Moq;
using WebApi.Models.Contracts;
using WebApi.Models.Services;

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
        public void GetAll_WhenCalled_ReturnsUsers()
        {
            //Arrange
            var referenceUsers = TestDataHelper.GetFakeUsersList();
            
            //Act
            var users = _manager.UserRepository.GetAll();

            //Assert
            Assert.That(users.Count(), Is.EqualTo(referenceUsers.Count));
        }

        [Test]
        public void GetById_WhenCalled_ReturnsUser()
        {
            //Arrange
            var referanceUser = TestDataHelper.GetFakeUsersList()[1];

            //Act
            var user = _manager.UserRepository.GetById(2);

            //Assert
            Assert.That(user, Is.Not.Null);
            Assert.That(user.Id, Is.EqualTo(referanceUser.Id));
        }

        [Test]
        public void GetByEmail_WhenCalled_ReturnsUser()
        {
            //Arrange
            var referanceUser = TestDataHelper.GetFakeUsersList()[1];

            //Act
            var user = _manager.UserRepository.GetByEmail("mark.lu@test.com");

            //Assert
            Assert.That(user, Is.Not.Null);
            Assert.That(user.Id, Is.EqualTo(referanceUser.Id));
        }

        [Test]
        public void Add_WhenCalled_AddsUser()
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
            _manager.UserRepository.Add(user);
            _manager.SaveChanges();
            var addedUser = _manager.UserRepository.GetByEmail(user.Email);

            //Assert
            Assert.That(addedUser, Is.Not.Null);
            Assert.That(addedUser.Email, Is.EqualTo(user.Email));
        }
    }
}