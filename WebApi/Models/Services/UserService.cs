using AutoMapper;
using DataAccess.Entities;
using DataAccess.Repositories.Abstractions;
using System.Security.Cryptography;
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
            this._repositoryManager = manager;
            this._mapper = mapper;
        }

        public void Create(UserForCreationDto dtoModel)
        {
            CreatePasswordHash(dtoModel.Password, out byte[] passwordHash, out byte[] passwordSalt);

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

        public void Update(UserModel model)
        {
            var entity = _mapper.Map<UserModel, User>(model);
            _repositoryManager.UserRepository.Update(entity);
            _repositoryManager.SaveChanges();
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public bool ComparePassword(UserModel user, string password)
        {
            using (var hmac = new HMACSHA512(user.PasswordSalt))
            {
                var inputPasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                if (inputPasswordHash == user.PasswordHash)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
