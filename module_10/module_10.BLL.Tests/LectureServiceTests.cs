using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using module_10.BLL.Exceptions;
using module_10.BLL.Services;
using module_10.DL.Interfaces;
using module_10.DL.Models;
using Moq;
using NUnit.Framework;

namespace module_10.BLL.Tests
{
    [TestFixture]
    public class LectureServiceTests
    {
        [Test]
        public void GetAllLectures_NoConditions_ReturnLectures()
        {
            var lectureRepository = new Mock<IRepository<Lecture>>();
            lectureRepository.Setup(x => x.GetAll()).Returns(() => new List<Lecture>()).Verifiable();
            var logger = new Mock<ILogger<LectureService>>();
            var lectureService = new LectureService(lectureRepository.Object, logger.Object);

            var lectureExpected = lectureService.GetAllLectures();
            Assert.That(lectureExpected, Is.Not.Null);
        }

        [Test]
        public void Get_LecturesIsNull_Throws_LecturesNotExistException()
        {
            var guid = Guid.NewGuid();
            var lectureRepository = new Mock<IRepository<Lecture>>();
            lectureRepository.Setup(x => x.Get(It.IsAny<Guid>())).Returns(() => null).Verifiable();
            var logger = new Mock<ILogger<LectureService>>();
            var lectureService = new LectureService(lectureRepository.Object, logger.Object);
            Assert.That(() => lectureService.Get(guid), Throws.Exception.TypeOf<LectureNotExistException>());
        }

        [Test]
        public void Get_LectureIsNotNull_Returns_Lecture()
        {
            var guid = Guid.NewGuid();
            var lecture = new Lecture() { Id = guid };
            var lectureRepository = new Mock<IRepository<Lecture>>();
            lectureRepository.Setup(x => x.Get(guid)).Returns(() => lecture).Verifiable();
            var logger = new Mock<ILogger<LectureService>>();
            var lectureService = new LectureService(lectureRepository.Object, logger.Object);
            Assert.That(() => lectureService.Get(guid), Is.EqualTo(lecture));
        }

        [Test]
        public void Create_LectureAlreadyExist_Throws_LectureAlreadyExistException()
        {
            var lecture = new Lecture
            {
                LectureName = "MyLecture"
            };
            var lectureRepository = new Mock<IRepository<Lecture>>();
            lectureRepository.Setup(x => x.GetAll()).Returns(() => new List<Lecture> { lecture }).Verifiable();
            var logger = new Mock<ILogger<LectureService>>();
            var lectureService = new LectureService(lectureRepository.Object, logger.Object);
            Assert.That(() => lectureService.Create(lecture), Throws.Exception.TypeOf<LectureAlreadyExistException>());
        }

        [Test]
        public void Create_LectureNotExist_Return_Lecture()
        {
            var homework = new Homework
            {
                Task = "Do job!"
            };
            var lecture = new Lecture
            {
                LectureName = "MyLecture",
                Homework = homework
            };
            var lectureRepository = new Mock<IRepository<Lecture>>();
            lectureRepository.Setup(x => x.GetAll()).Returns(() => new List<Lecture>()).Verifiable();
            lectureRepository.Setup(x => x.Create(lecture)).Returns(() => lecture).Verifiable();
            var logger = new Mock<ILogger<LectureService>>();
            var lectureService = new LectureService(lectureRepository.Object, logger.Object);

            Assert.That(() => lectureService.Create(lecture).LectureName, Is.EqualTo(lecture.LectureName));
            Assert.That(() => lectureService.Create(lecture).Homework, Is.EqualTo(homework));
        }

        [Test]
        public void Update_Lecture_LectureIsNotExist_Throws_LectureNotExistException()
        {
            var guid = Guid.NewGuid();
            var lecture = new Lecture();
            var lectureRepository = new Mock<IRepository<Lecture>>();
            lectureRepository.Setup(x => x.Update(lecture)).Returns(() => null).Verifiable();
            var logger = new Mock<ILogger<LectureService>>();
            var lectureService = new LectureService(lectureRepository.Object, logger.Object);
            Assert.That(() => lectureService.Update(guid, lecture), Throws.Exception.TypeOf<LectureNotExistException>());
        }

        [Test]
        public void Update_Lecture_LectureExist_Returns_Lecture()
        {
            var guid = Guid.NewGuid();
            var lecture = new Lecture()
            {
                LectureName = "Lecture"
            };
            var lectureRepository = new Mock<IRepository<Lecture>>();
            lectureRepository.Setup(x => x.Update(lecture)).Returns(() => lecture).Verifiable();
            var logger = new Mock<ILogger<LectureService>>();
            var lectureService = new LectureService(lectureRepository.Object, logger.Object);
            Assert.That(() => lectureService.Update(guid, lecture), Is.EqualTo(lecture));
        }
        [Test]
        public void Delete_Lecture_LectureNotExist_Throws_LectureNotExistException()
        {
            var lectureRepository = new Mock<IRepository<Lecture>>();
            lectureRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(() => false).Verifiable();
            var logger = new Mock<ILogger<LectureService>>();
            var lectureService = new LectureService(lectureRepository.Object, logger.Object);
            Assert.That(() => lectureService.Delete(It.IsAny<Guid>()), Throws.Exception.TypeOf<LectureNotExistException>());
        }

        [Test]
        public void Delete_Lecture_LectureExist_Return_True()
        {
            var guid = Guid.NewGuid();
            var lectureRepository = new Mock<IRepository<Lecture>>();
            lectureRepository.Setup(x => x.Delete(guid)).Returns(() => true).Verifiable();
            var logger = new Mock<ILogger<LectureService>>();
            var lectureService = new LectureService(lectureRepository.Object, logger.Object);
            var result = lectureService.Delete(guid);
            Assert.That(result, Is.True);
            lectureRepository.Verify();
        }
    }
}
