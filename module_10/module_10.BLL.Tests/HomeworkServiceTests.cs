using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using module_10.BLL.Exceptions;
using module_10.BLL.Services;
using module_10.DL.Interfaces;
using module_10.DL.Models;
using Moq;
using NUnit.Framework;
using ILogger = Microsoft.Extensions.Logging.ILogger;
namespace module_10.BLL.Tests
{
    [TestFixture]
    public class HomeworkServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetAllHomeworks_NoConditions_ReturnHomeworks()
        {
            var homeworkRepository = new Mock<IRepository<Homework>>();
            homeworkRepository.Setup(x => x.GetAll()).Returns(() => new List<Homework>()).Verifiable();
            var logger = new Mock<ILogger<HomeworkService>>();
            var lectureService = new Mock<ILectureService<Lecture>>();
            var homeworkService = new HomeworkService(homeworkRepository.Object, logger.Object, lectureService.Object);

            var homeworksExpected = homeworkService.GetAllHomeworks();

            Assert.That(homeworksExpected, Is.Not.Null);
        }

        [Test]
        public void Get_HomeworkIsNull_Throws_HomeworkNotExistException()
        {
            var guid = Guid.NewGuid();
            var homeworkRepository = new Mock<IRepository<Homework>>();
            homeworkRepository.Setup(x => x.Get(It.IsAny<Guid>())).Returns(() => null).Verifiable();
            var logger = new Mock<ILogger<HomeworkService>>();
            var lectureService = new Mock<ILectureService<Lecture>>();
            var homeworkService = new HomeworkService(homeworkRepository.Object, logger.Object, lectureService.Object);

            Assert.That(() => homeworkService.Get(guid), Throws.Exception.TypeOf<HomeworkNotExistException>());
        }

        [Test]
        public void Get_HomeworkIsNotNull_Returns_Homework()
        {
            var guid = Guid.NewGuid();
            var homework = new Homework { Id = guid };
            var homeworkRepository = new Mock<IRepository<Homework>>();
            homeworkRepository.Setup(x => x.Get(guid)).Returns(() => homework).Verifiable();
            var logger = new Mock<ILogger<HomeworkService>>();
            var lectureService = new Mock<ILectureService<Lecture>>();
            var homeworkService = new HomeworkService(homeworkRepository.Object, logger.Object, lectureService.Object);

            Assert.That(() => homeworkService.Get(guid), Is.EqualTo(homework));
        }
        [Test]
        public void Create_LectureIsNull_Throws_LectureNotExistException()
        {
            var homework = new Homework();
            var homeworkRepository = new Mock<IRepository<Homework>>();
            var logger = new Mock<ILogger<HomeworkService>>();
            var lectureService = new Mock<ILectureService<Lecture>>();
            lectureService.Setup(x => x.Get(It.IsAny<Guid>())).Returns(() => null).Verifiable();
            var homeworkService = new HomeworkService(homeworkRepository.Object, logger.Object, lectureService.Object);

            Assert.That(() => homeworkService.Create(homework), Throws.Exception.TypeOf<LectureNotExistException>());
            lectureService.Verify();
        }

        [Test]
        public void Create_HomeworkLectureIsNotNull_Throws_HomeworkAlreadyExistException()
        {
            var lecture = new Lecture
            {
                Homework = new Homework()
            };

            var homework = new Homework();
            var homeworkRepository = new Mock<IRepository<Homework>>();
            var logger = new Mock<ILogger<HomeworkService>>();
            var lectureService = new Mock<ILectureService<Lecture>>();
            lectureService.Setup(x => x.Get(It.IsAny<Guid>())).Returns(() => lecture).Verifiable();
            var homeworkService = new HomeworkService(homeworkRepository.Object, logger.Object, lectureService.Object);

            Assert.That(() => homeworkService.Create(homework), Throws.Exception.TypeOf<HomeworkAlreadyExistException>());
            lectureService.Verify();
        }

        [Test]
        public void Create_Homework_IsNotNull_And_Lecture_IsNotNull_Returns_Homework()
        {
            var guid = Guid.NewGuid();
            var lecture = new Lecture
            {
                Id = guid,
                LectureName = "MyLecture"
            };

            var homework = new Homework()
            {
                LectureId = guid
            };
            var homeworkRepository = new Mock<IRepository<Homework>>();
            homeworkRepository.Setup(x => x.Create(homework)).Returns(() => homework).Verifiable();
            var logger = new Mock<ILogger<HomeworkService>>();
            var lectureService = new Mock<ILectureService<Lecture>>();
            lectureService.Setup(x => x.Get(guid)).Returns(() => lecture).Verifiable();
            var homeworkService = new HomeworkService(homeworkRepository.Object, logger.Object, lectureService.Object);

            Assert.That(() => homeworkService.Create(homework).LectureId, Is.EqualTo(lecture.Id));
            Assert.That(() => homework.LectureId, Is.EqualTo(homeworkService.Create(homework).LectureId));
            homeworkRepository.Verify();
            lectureService.Verify();
        }

        [Test]
        public void Update_Homework_HomeworkIsNotExist_Throws_HomeworkNotExistException()
        {
            var guid = Guid.NewGuid();
            var homework = new Homework();
            var homeworkRepository = new Mock<IRepository<Homework>>();
            homeworkRepository.Setup(x => x.Update(homework)).Returns(() => null).Verifiable();
            var logger = new Mock<ILogger<HomeworkService>>();
            var lectureService = new Mock<ILectureService<Lecture>>();
            var homeworkService = new HomeworkService(homeworkRepository.Object, logger.Object, lectureService.Object);

            Assert.That(() => homeworkService.Update(guid, homework), Throws.Exception.TypeOf<HomeworkNotExistException>());
            homeworkRepository.Verify();
        }

        [Test]
        public void Update_Homework_HomeworkExist_Returns_Homework()
        {
            var guid = Guid.NewGuid();
            var homework = new Homework();
            var homeworkRepository = new Mock<IRepository<Homework>>();
            homeworkRepository.Setup(x => x.Update(homework)).Returns(() => homework).Verifiable();
            var logger = new Mock<ILogger<HomeworkService>>();
            var lectureService = new Mock<ILectureService<Lecture>>();
            var homeworkService = new HomeworkService(homeworkRepository.Object, logger.Object, lectureService.Object);

            Assert.That(() => homeworkService.Update(guid, homework), Is.EqualTo(homework));
            homeworkRepository.Verify();
        }
        [Test]
        public void Delete_Homework_HomeworkNotExist_Throws_HomeworkNotExistException()
        {
            var homeworkRepository = new Mock<IRepository<Homework>>();
            homeworkRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(() => false).Verifiable();
            var logger = new Mock<ILogger<HomeworkService>>();
            var lectureService = new Mock<ILectureService<Lecture>>();
            var homeworkService = new HomeworkService(homeworkRepository.Object, logger.Object, lectureService.Object);
            Assert.That(() => homeworkService.Delete(It.IsAny<Guid>()), Throws.Exception.TypeOf<HomeworkNotExistException>());
            homeworkRepository.Verify();
        }
        [Test]
        public void Delete_Homework_HomeworkExist_Return_True()
        {
            var guid = Guid.NewGuid();
            var homeworkRepository = new Mock<IRepository<Homework>>();
            homeworkRepository.Setup(x => x.Delete(guid)).Returns(() => true).Verifiable();
            var logger = new Mock<ILogger<HomeworkService>>();
            var lectureService = new Mock<ILectureService<Lecture>>();
            var homeworkService = new HomeworkService(homeworkRepository.Object, logger.Object, lectureService.Object);
            var result = homeworkService.Delete(guid);
            Assert.That(result, Is.True);
            homeworkRepository.Verify();
        }
    }
}