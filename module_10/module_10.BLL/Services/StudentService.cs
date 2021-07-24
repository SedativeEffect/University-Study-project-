using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using module_10.BLL.Exceptions;
using module_10.BLL.Utils;
using module_10.DL.Interfaces;
using module_10.DL.Models;

namespace module_10.BLL.Services
{
    public class StudentService : IStudentService<User>
    {
        private readonly IRepository<User> _usersRepository;
        private readonly ILogger<StudentService> _logger;

        public StudentService(IRepository<User> usersRepository, ILogger<StudentService> logger)
        {
            _usersRepository = usersRepository;
            _logger = logger;
        }
        public IEnumerable<User> GetAllStudents()
        {
            return _usersRepository.GetAll();
        }

        public User Get(Guid id)
        {
            var user = _usersRepository.Get(id);
            if (user is null)
            {
                _logger.LogAndThrow(new UserNotExistException($"User with id {id} not exist"), LogLevel.Warning);
            }
            return user;
        }

        public User Create(User student)
        {
            var user = _usersRepository.Create(student);
            if (user is null)
            {
                _logger.LogAndThrow(new UserAlreadyExistException($"User with username {student.Username} already exist"), LogLevel.Warning);
            }
            return user;
        }

        public User Update(Guid id, User student)
        {
            student.Id = id;
            var user = _usersRepository.Update(student);
            if (user is null)
            {
                _logger.LogAndThrow(new UserNotExistException($"User with id {id} not exist"), LogLevel.Warning);
            }
            return user;
        }

        public bool Delete(Guid id)
        {
            var isDelete = _usersRepository.Delete(id);
            if (!isDelete)
            {
                _logger.LogAndThrow(new UserNotExistException($"User with id {id} not exist"), LogLevel.Warning);
            }
            return isDelete;
        }

        
    }
}
