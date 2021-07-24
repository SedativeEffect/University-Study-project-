using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using module_10.BLL.Exceptions;
using module_10.BLL.Models;
using module_10.BLL.Services;
using module_10.DL.Interfaces;
using module_10.DL.Models;
using Moq;
using NUnit.Framework;

namespace module_10.BLL.Tests
{
    [TestFixture]
    public class StudentServiceTests
    {
        [Test]
        public void GetAllStudents_NoConditions_ReturnStudents()
        {
            var userRepository = new Mock<IRepository<User>>();
            userRepository.Setup(x => x.GetAll()).Returns(() => new List<User>()).Verifiable();
            var logger = new Mock<ILogger<StudentService>>();
            var studentService = new StudentService(userRepository.Object, logger.Object);

            var usersExpected = studentService.GetAllStudents();
            Assert.That(usersExpected, Is.Not.Null);
        }
        [Test]
        public void Get_UserIsNull_Throws_UserNotExistException()
        {
            var guid = Guid.NewGuid();
            var userRepository = new Mock<IRepository<User>>();
            userRepository.Setup(x => x.Get(It.IsAny<Guid>())).Returns(() => null).Verifiable();
            var logger = new Mock<ILogger<StudentService>>();
            var studentService = new StudentService(userRepository.Object, logger.Object);
            Assert.That(() => studentService.Get(guid), Throws.Exception.TypeOf<UserNotExistException>());
        }

        [Test]
        public void Get_UserIsNotNull_Returns_User()
        {
            var guid = Guid.NewGuid();
            var user = new User() { Id = guid };
            var userRepository = new Mock<IRepository<User>>();
            userRepository.Setup(x => x.Get(guid)).Returns(() => user).Verifiable();
            var logger = new Mock<ILogger<StudentService>>();
            var studentService = new StudentService(userRepository.Object, logger.Object);
            Assert.That(() => studentService.Get(guid), Is.EqualTo(user));
        }

        [Test]
        public void Create_UserAlreadyExist_Throws_UserAlreadyExistException()
        {
            var user = new User
            {
                Username = "Student"
            };
            var userRepository = new Mock<IRepository<User>>();
            userRepository.Setup(x => x.GetAll()).Returns(() => new List<User> { user }).Verifiable();
            var logger = new Mock<ILogger<StudentService>>();
            var studentService = new StudentService(userRepository.Object, logger.Object);
            Assert.That(() => studentService.Create(user), Throws.Exception.TypeOf<UserAlreadyExistException>());
        }
        [Test]
        public void Create_UserNotExist_Return_User()
        {
            var user = new User()
            {
                Username = "Student"
            };
            var userRepository = new Mock<IRepository<User>>();
            userRepository.Setup(x => x.GetAll()).Returns(() => new List<User>()).Verifiable();
            userRepository.Setup(x => x.Create(user)).Returns(() => user).Verifiable();
            var logger = new Mock<ILogger<StudentService>>();
            var studentService = new StudentService(userRepository.Object, logger.Object);
            Assert.That(() => studentService.Create(user).Username, Is.EqualTo(user.Username));
        }

        [Test]
        public void Update_User_UserIsNotExist_Throws_UserNotExistException()
        {
            var guid = Guid.NewGuid();
            var user = new User();
            var userRepository = new Mock<IRepository<User>>();
            userRepository.Setup(x => x.Update(user)).Returns(() => null).Verifiable();
            var logger = new Mock<ILogger<StudentService>>();
            var studentService = new StudentService(userRepository.Object, logger.Object);
            Assert.That(() => studentService.Update(guid, user), Throws.Exception.TypeOf<UserNotExistException>());
        }

        [Test]
        public void Update_User_UserExist_Returns_User()
        {
            var guid = Guid.NewGuid();
            var user = new User
            {
                Username = "Student"
            };
            var userRepository = new Mock<IRepository<User>>();
            userRepository.Setup(x => x.Update(user)).Returns(() => user).Verifiable();
            var logger = new Mock<ILogger<StudentService>>();
            var studentService = new StudentService(userRepository.Object, logger.Object);
            Assert.That(() => studentService.Update(guid, user), Is.EqualTo(user));
        }

        [Test]
        public void Delete_User_UserNotExist_Throws_UserNotExistException()
        {
            var userRepository = new Mock<IRepository<User>>();
            userRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(() => false).Verifiable();
            var logger = new Mock<ILogger<StudentService>>();
            var studentService = new StudentService(userRepository.Object, logger.Object);
            Assert.That(() => studentService.Delete(It.IsAny<Guid>()), Throws.Exception.TypeOf<UserNotExistException>());
        }
        [Test]
        public void Delete_User_UserExist_Return_True()
        {
            var guid = Guid.NewGuid();
            var userRepository = new Mock<IRepository<User>>();
            userRepository.Setup(x => x.Delete(guid)).Returns(() => true).Verifiable();
            var logger = new Mock<ILogger<StudentService>>();
            var studentService = new StudentService(userRepository.Object, logger.Object);
            var result = studentService.Delete(guid);
            Assert.That(result, Is.True);
            userRepository.Verify();
        }
    }
}
