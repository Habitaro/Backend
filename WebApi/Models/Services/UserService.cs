﻿using AutoMapper;
using DataAccess.Entities;
using DataAccess.Repositories.Abstractions;
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

        public void Create(UserModel model)
        {
            var entity = _mapper.Map<UserModel, User>(model);
            if (entity  != null) 
            {
                _repositoryManager.UserRepository.Add(entity);
                _repositoryManager.SaveChanges();
            }
        }

        public IEnumerable<UserModel> GetAll()
        {
            var entities = _repositoryManager.UserRepository.GetAll();
            var models = _mapper.Map<IEnumerable<User>, IEnumerable<UserModel>>(entities);
            return models;
        }

        public UserModel? GetByEmail(string email)
        {
            throw new NotImplementedException();
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
    }
}