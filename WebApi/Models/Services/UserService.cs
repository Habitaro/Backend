using AutoMapper;
using DataAccess.Entities;
using DataAccess.Repositories.Abstractions;
using System.Security.Cryptography;
using System.Text;
using WebApi.Models.Contracts;
using WebApi.Models.Services.Abstractions;

namespace WebApi.Models.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryManager _repositoryManager;

        private readonly IMapper _mapper;

        public UserService(IRepositoryManager manager, IMapper mapper)
        {
            _repositoryManager = manager;
            _mapper = mapper;
        }

        public void Create(UserForCreationDto dtoModel)
        {
            CreatePasswordHash(dtoModel.Password, out string passwordHash, out string passwordSalt);

            var model = new UserModel()
            {
                Username = dtoModel.Username,
                Email = dtoModel.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = "",
                AvatarId = 0,
                RankId = 1,
            };

            var entity = _mapper.Map<UserModel, User>(model);

            _repositoryManager.UserRepository.Add(entity);
            _repositoryManager.SaveChanges();

        }

        public IEnumerable<UserModel> GetAll()
        {
            var entities = _repositoryManager.UserRepository.GetAll();
            var models = _mapper.Map<IEnumerable<User>, IEnumerable<UserModel>>(entities);
            return models;
        }

        public UserModel? GetByEmail(string email)
        {
            var entity = _repositoryManager.UserRepository.GetByEmail(email);
            if (entity != null)
            {
                var model = _mapper.Map<User, UserModel>(entity);
                return model;
            }

            return null;
        }

        public UserModel? GetById(int id)
        {
            var entity = _repositoryManager.UserRepository.GetById(id);
            if (entity != null)
            {
                var model = _mapper.Map<User, UserModel>(entity);
                return model;
            }

            return null;
        }

        public void Remove(UserModel model)
        {
            var entity = _mapper.Map<UserModel, User>(model);
            _repositoryManager.UserRepository.Delete(entity);
            _repositoryManager.SaveChanges();
        }

        public void RemoveById(int id)
        {
            var entity = _repositoryManager.UserRepository.GetById(id) ?? throw new ArgumentNullException(nameof(id));
            _repositoryManager.UserRepository.Delete(entity);
            _repositoryManager.SaveChanges();
        }

        public void Update(UserForEditDto dtoModel, int id)
        {
            var user = _repositoryManager.UserRepository.GetById(id)!;
            user.AvatarId = dtoModel.AvatarId ?? user.AvatarId;
            user.Status = dtoModel.Status ?? user.Status;
            user.Username = dtoModel.Username ?? user.Username;

            _repositoryManager.UserRepository.Update(user);
            _repositoryManager.SaveChanges();
        }

        private static void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                passwordSalt = Convert.ToBase64String(hmac.Key);
                passwordHash = Convert.ToBase64String(computedHash);
            }
        }

        public bool VerifyPassword(UserModel user, string password)
        {
            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(user.PasswordSalt)))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                return user.PasswordHash == Convert.ToBase64String(computedHash);
            }
        }
    }
}
