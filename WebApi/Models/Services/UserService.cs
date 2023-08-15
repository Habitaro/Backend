using AutoMapper;
using DataAccess.Entities;
using DataAccess.Repositories.Abstractions;
using System.Security.Cryptography;
using System.Text;
using WebApi.Models.Contracts;
using WebApi.Models.Services.Abstractions;
using WebApi.Models.Services.Helpers;

namespace WebApi.Models.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryManager _repositoryManager;

        private readonly IConfiguration _configuration;

        private readonly IMapper _mapper;

        private const int _iterations = 3;

        public UserService(IRepositoryManager manager, IMapper mapper, IConfiguration configuration)
        {
            _repositoryManager = manager;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task Create(UserCreationDto dtoModel)
        {
            try
            {
                if (await GetByEmailAsDto(dtoModel.Email) != null)
                {
                    throw new InvalidOperationException(message: $"The {dtoModel.Email} email is already registered");
                }
            }
            catch (ArgumentNullException ex)
            {
                if (ex.Message != $"User with Email {dtoModel.Email} was not found")
                {
                    throw;
                }
            }

            var salt = HashHelper.GenerateSalt();
            var passwordHash = HashHelper.ComputeHash(dtoModel.Password, salt, _configuration["PasswordPepper"], _iterations);

            var model = new UserModel()
            {
                UserName = dtoModel.Username,
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

        public async Task<UserReadDto> GetByEmailAsDto(string email)
        {
            var entity = await _repositoryManager.UserRepository.GetByEmail(email)
                ?? throw new ArgumentNullException(message: $"User with Email {email} was not found", null);

            var dto = _mapper.Map<User, UserReadDto>(entity);
            return dto;
        }

        public async Task<UserReadDto> GetByIdAsDto(int id)
        {
            var entity = await _repositoryManager.UserRepository.GetByIdAsNoTracking(id)
                ?? throw new ArgumentNullException(message: $"User with Id {id} was not found", null);
            var dto = _mapper.Map<User, UserReadDto>(entity);
            return dto;
        }

        public async Task<IEnumerable<UserModel>> GetAllAsModel()
        {
            var entities = await _repositoryManager.UserRepository.GetAll();
            var models = _mapper.Map<IEnumerable<User>, IEnumerable<UserModel>>(entities);
            return models;
        }

        public async Task<UserModel> GetByEmailAsModel(string email)
        {
            var entity = await _repositoryManager.UserRepository.GetByEmail(email)
                ?? throw new ArgumentNullException(message: $"User with Email {email} was not found", null);

            var model = _mapper.Map<User, UserModel>(entity);
            return model;
        }

        public async Task<UserModel> GetByIdAsModel(int id)
        {
            var entity = await _repositoryManager.UserRepository.GetById(id)
                ?? throw new ArgumentNullException(message: $"User with Id {id} was not found", null);

            var model = _mapper.Map<User, UserModel>(entity);
            return model;
        }

        public async Task Remove(UserModel model)
        {
            var entity = await _repositoryManager.UserRepository.GetById(model.Id)
                ?? throw new ArgumentNullException(message:$"User with Id {model.Id} was not found", null);
            _repositoryManager.UserRepository.Delete(entity);
            await _repositoryManager.SaveChanges();
        }

        public async Task RemoveById(int id)
        {
            var entity = await _repositoryManager.UserRepository.GetById(id) 
                ?? throw new ArgumentNullException(message: $"User with Id {id} was not found", null);
            _repositoryManager.UserRepository.Delete(entity);
            await _repositoryManager.SaveChanges();
        }

        public async Task Update(UserEditDto dtoModel, int id)
        {
            var user = await _repositoryManager.UserRepository.GetById(id)
                ?? throw new ArgumentNullException(message: $"User with Id {id} was not found", null);
            user.AvatarId = dtoModel.AvatarId ?? user.AvatarId;
            user.Status = dtoModel.Status ?? user.Status;
            user.UserName = dtoModel.Username ?? user.UserName;

            _repositoryManager.UserRepository.Update(user);
            await _repositoryManager.SaveChanges();
        }

        public bool VerifyPassword(UserModel model, string password)
        {

            var computedHash = HashHelper.ComputeHash(password, model.PasswordSalt, _configuration["PasswordPepper"], _iterations);

            return model.PasswordHash.SequenceEqual(computedHash);
        }

        public async Task AddExp(int id, int exp)
        {
            var entity = await _repositoryManager.UserRepository.GetById(id) 
                ?? throw new ArgumentNullException(message: $"User with Id {id} was not found", null);
            entity.CurrentExp += exp;

            if ((entity.CurrentExp >= entity.RequiredExp) && entity.RankId < 8)
            {
                entity.RankId += 1;
                entity.CurrentExp -= entity.RequiredExp;
                entity.RequiredExp = (int)(entity.RequiredExp * 1.5);
                entity.RequiredExp -= entity.RequiredExp % 10;
            }

            await _repositoryManager.SaveChanges();
        }
    }
}
