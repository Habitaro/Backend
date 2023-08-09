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

        public async Task Create(UserCreationDto dtoModel, string pepper)
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

            await _repositoryManager.UserRepository.Add(entity);
            await _repositoryManager.SaveChanges();

        }

        public async Task<IEnumerable<UserReadDto>> GetAllAsDto()
        {
            var entities = await _repositoryManager.UserRepository.GetAll();
            var dtos = _mapper.Map<IEnumerable<User>, IEnumerable<UserReadDto>>(entities);
            return dtos;
        }

        public async Task<UserReadDto?> GetByEmailAsDto(string email)
        {
            var entity = await _repositoryManager.UserRepository.GetByEmail(email);
            if (entity != null)
            {
                var dto = _mapper.Map<User, UserReadDto>(entity);
                return dto;
            }

            return null;
        }

        public async Task<UserReadDto?> GetByIdAsDto(int id)
        {
            var entity = await _repositoryManager.UserRepository.GetById(id);
            if (entity != null)
            {
                var dto = _mapper.Map<User, UserReadDto>(entity);
                return dto;
            }
            return null;
        }

        public async Task<IEnumerable<UserModel>> GetAllAsModel()
        {
            var entities = await _repositoryManager.UserRepository.GetAll();
            var models = _mapper.Map<IEnumerable<User>, IEnumerable<UserModel>>(entities);
            return models;
        }

        public async Task<UserModel?> GetByEmailAsModel(string email)
        {
            var entity = await _repositoryManager.UserRepository.GetByEmail(email);
            if (entity != null)
            {
                var model = _mapper.Map<User, UserModel>(entity);
                return model;
            }
            return null;
        }

        public async Task<UserModel?> GetByIdAsModel(int id)
        {
            var entity = await _repositoryManager.UserRepository.GetById(id);
            if (entity != null)
            {
                var model = _mapper.Map<User, UserModel>(entity);
                return model;
            }
            return null;
        }

        public async Task Remove(UserModel model)
        {
            var entity = _mapper.Map<UserModel, User>(model);
            _repositoryManager.UserRepository.Delete(entity);
            await _repositoryManager.SaveChanges();
        }

        public async Task RemoveById(int id)
        {
            var entity = await _repositoryManager.UserRepository.GetById(id) ?? throw new ArgumentNullException(nameof(id));
            _repositoryManager.UserRepository.Delete(entity);
            await _repositoryManager.SaveChanges();
        }

        public async Task Update(UserEditDto dtoModel, int id)
        {
            var user = await _repositoryManager.UserRepository.GetById(id) ?? throw new ArgumentNullException(nameof(dtoModel));
            user.AvatarId = dtoModel.AvatarId ?? user.AvatarId;
            user.Status = dtoModel.Status ?? user.Status;
            user.UserName = dtoModel.Username ?? user.UserName;

            _repositoryManager.UserRepository.Update(user);
            await _repositoryManager.SaveChanges();
        }

        public bool VerifyPassword(UserModel model, string password, string pepper)
        {

            var computedHash = HashHelper.ComputeHash(password, model.PasswordSalt, pepper, _iterations);

            return model.PasswordHash.SequenceEqual(computedHash);
        }

        public async Task AddExp(UserModel model, int exp)
        {
            var entity = await _repositoryManager.UserRepository.GetById(model.Id) ?? throw new ArgumentNullException(nameof(model));
            entity.CurrentExp += exp;

            if ((entity.CurrentExp >= entity.RequiredExp) && entity.RankId < 8)
            {
                entity.CurrentExp -= entity.RequiredExp;
                entity.RequiredExp = (int)(entity.RequiredExp * 1.5);
                entity.RequiredExp -= entity.RequiredExp % 10;
            }

            await _repositoryManager.SaveChanges();
        }
    }
}
