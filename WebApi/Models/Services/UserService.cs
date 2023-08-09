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

        private const int _iterations = 3;

        public UserService(IRepositoryManager manager, IMapper mapper)
        {
            _repositoryManager = manager;
            _mapper = mapper;
        }

        public void Create(UserForCreationDto dtoModel, string pepper)
        {
            var salt = HashHelper.GenerateSalt();
            var passwordHash = HashHelper.ComputeHash(dtoModel.Password, salt, pepper, _iterations);

            var model = new UserModel()
            {
                Username = dtoModel.Username,
                Email = dtoModel.Email,
                PasswordHash = passwordHash,
                PasswordSalt = salt,
                Status = "",
                AvatarId = 0,
                RankId = 1,
                RequiredExp = 1000,
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
            user.UserName = dtoModel.Username ?? user.UserName;

            _repositoryManager.UserRepository.Update(user);
            _repositoryManager.SaveChanges();
        }

        public bool VerifyPassword(UserModel model, string password, string pepper)
        {

            var computedHash = HashHelper.ComputeHash(password, model.PasswordSalt, pepper, _iterations);

            return model.PasswordHash.SequenceEqual(computedHash);
        }

        public void AddExp(UserModel model, int exp)
        {
            var entity = _repositoryManager.UserRepository.GetById(model.Id) ?? throw new ArgumentNullException(nameof(model));
            entity.CurrentExp += exp;

            if ((entity.CurrentExp >= entity.RequiredExp) && entity.RankId < 8)
            {
                entity.CurrentExp -= entity.RequiredExp;
                entity.RequiredExp = (int)(entity.RequiredExp * 1.5);
                entity.RequiredExp -= entity.RequiredExp % 10;
            }

            _repositoryManager.SaveChanges();
        }
    }
}
