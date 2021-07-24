using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using module_10.BLL.Exceptions;
using module_10.BLL.Services;
using module_10.BLL.Utils;
using module_10.DAL.University;
using module_10.DAL.University.Entities;
using module_10.DL.Interfaces;
using module_10.DL.Models;
using Moq;
using NUnit.Framework;
using Journal = module_10.DAL.University.Entities.Journal;
using Lecture = module_10.DL.Models.Lecture;

namespace module_10.BLL.Tests
{
    [TestFixture]
    public class StudyServiceTests
    {
        private DbContextOptions<UniversityContext> _options;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<UniversityContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }
        [Test]
        public void AddStudentToLecture_LectureIsNull_Throws_LectureNotExistException()
        {
            var usersRepository = new Mock<IRepository<User>>();

            var lecturesRepository = new Mock<IRepository<Lecture>>();

            var logger = new Mock<ILogger<StudyService>>();
            var guid = Guid.NewGuid();
            using (var context = new UniversityContext(_options))
            {
                context.Lectures.Add(new()
                {
                    Id = guid,
                    LectureName = "Lecture"
                });
                context.SaveChanges();
            }

            using (var context = new UniversityContext(_options))
            {
                var mapper = new Mock<IMapper>();
                var notifyService = new Mock<IEmailService>();

                var smsService = new Mock<ISmsService>();
                var studyService = new StudyService(usersRepository.Object, lecturesRepository.Object, logger.Object,
                    context, mapper.Object, notifyService.Object, smsService.Object);
                Assert.That(() => studyService.AddStudentToLecture(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>()), Throws.Exception.TypeOf<LectureNotExistException>());
            }

        }
        [Test]
        public void AddStudentToLecture_LectureHomeworkIsNull_Throws_HomeworkNotExistException()
        {
            var usersRepository = new Mock<IRepository<User>>();

            var lecturesRepository = new Mock<IRepository<Lecture>>();

            var logger = new Mock<ILogger<StudyService>>();
            var guid = Guid.NewGuid();
            using (var context = new UniversityContext(_options))
            {
                context.Lectures.Add(new()
                {
                    Id = guid,
                    LectureName = "Lecture",
                    Homework = null
                });
                context.SaveChanges();
            }

            using (var context = new UniversityContext(_options))
            {
                var mapper = new Mock<IMapper>();
                var notifyService = new Mock<IEmailService>();
                var smsService = new Mock<ISmsService>();
                var studyService = new StudyService(usersRepository.Object, lecturesRepository.Object, logger.Object,
                    context, mapper.Object, notifyService.Object, smsService.Object);
                Assert.That(() => studyService.AddStudentToLecture(It.IsAny<Guid>(), guid, It.IsAny<bool>()), Throws.Exception.TypeOf<HomeworkNotExistException>());
            }

        }

        [Test]
        public void AddStudentToLecture_UserIsNull_Throws_UserNotExistException()
        {
            var usersRepository = new Mock<IRepository<User>>();

            var lecturesRepository = new Mock<IRepository<Lecture>>();
            var homework = new DAL.University.Entities.Homework()
            {
                Id = Guid.NewGuid()
            };
            var logger = new Mock<ILogger<StudyService>>();
            var guid = Guid.NewGuid();
            using (var context = new UniversityContext(_options))
            {
                context.Lectures.Add(new()
                {
                    Id = guid,
                    LectureName = "Lecture",
                    Homework = homework
                });

                context.SaveChanges();
            }

            using (var context = new UniversityContext(_options))
            {
                var mapper = new Mock<IMapper>();
                var notifyService = new Mock<IEmailService>();
                var smsService = new Mock<ISmsService>();
                var studyService = new StudyService(usersRepository.Object, lecturesRepository.Object, logger.Object,
                    context, mapper.Object, notifyService.Object, smsService.Object);
                Assert.That(() => studyService.AddStudentToLecture(It.IsAny<Guid>(), guid, It.IsAny<bool>()), Throws.Exception.TypeOf<UserNotExistException>());
            }

        }

        [Test]
        public void AddStudentToLecture_LectureContainsUser_Throws_UserAlreadyExistException()
        {
            var usersRepository = new Mock<IRepository<User>>();

            var lecturesRepository = new Mock<IRepository<Lecture>>();
            var homework = new DAL.University.Entities.Homework()
            {
                Id = Guid.NewGuid()
            };
            var logger = new Mock<ILogger<StudyService>>();
            var guid = Guid.NewGuid();
            var guidUser = Guid.NewGuid();
            var user = new DAL.University.Identity.User()
            {
                Id = guidUser,
                UserName = "Denis"
            };
            using (var context = new UniversityContext(_options))
            {
                context.Users.Add(user);
                context.Lectures.Add(new()
                {
                    Id = guid,
                    LectureName = "Lecture",
                    Homework = homework,
                    Students = new List<DAL.University.Identity.User> { user }
                });

                context.SaveChanges();
            }

            using (var context = new UniversityContext(_options))
            {
                var mapper = new Mock<IMapper>();
                var notifyService = new Mock<IEmailService>();
                var smsService = new Mock<ISmsService>();
                var studyService = new StudyService(usersRepository.Object, lecturesRepository.Object, logger.Object,
                    context, mapper.Object, notifyService.Object, smsService.Object);
                Assert.That(() => studyService.AddStudentToLecture(guidUser, guid, It.IsAny<bool>()), Throws.Exception.TypeOf<UserAlreadyExistException>());
            }

        }

        [Test]
        public void AddStudentToLecture_NoLectorAtLecture_Throws_NoLectorAtLectureException()
        {
            var usersRepository = new Mock<IRepository<User>>();

            var lecturesRepository = new Mock<IRepository<Lecture>>();
            var homework = new DAL.University.Entities.Homework()
            {
                Id = Guid.NewGuid()
            };
            var logger = new Mock<ILogger<StudyService>>();
            var guid = Guid.NewGuid();
            var guidUser = Guid.NewGuid();
            var user = new DAL.University.Identity.User()
            {
                Id = guidUser,
                UserName = "Denis",
                Role = "student"
            };
            using (var context = new UniversityContext(_options))
            {
                context.Users.Add(user);
                context.Lectures.Add(new()
                {
                    Id = guid,
                    LectureName = "Lecture",
                    Homework = homework,
                    Students = new List<DAL.University.Identity.User>()
                });

                context.SaveChanges();
            }

            using (var context = new UniversityContext(_options))
            {
                var mapper = new Mock<IMapper>();
                var notifyService = new Mock<IEmailService>();
                var smsService = new Mock<ISmsService>();
                var studyService = new StudyService(usersRepository.Object, lecturesRepository.Object, logger.Object,
                    context, mapper.Object, notifyService.Object, smsService.Object);
                Assert.That(() => studyService.AddStudentToLecture(guidUser, guid, It.IsAny<bool>()), Throws.Exception.TypeOf<NoLectorAtLectureException>());
            }

        }

        [Test]
        public void AddStudentToLecture_LectorAlreadyExistAtLecture_Throws_UserAlreadyExistException()
        {
            var usersRepository = new Mock<IRepository<User>>();

            var lecturesRepository = new Mock<IRepository<Lecture>>();
            var homework = new DAL.University.Entities.Homework()
            {
                Id = Guid.NewGuid()
            };
            var logger = new Mock<ILogger<StudyService>>();
            var guid = Guid.NewGuid();
            var guidUser = Guid.NewGuid();
            var user = new DAL.University.Identity.User()
            {
                Id = guidUser,
                UserName = "Denis",
                Role = "lector"
            };
            using (var context = new UniversityContext(_options))
            {
                context.Users.Add(user);
                context.Lectures.Add(new()
                {
                    Id = guid,
                    LectureName = "Lecture",
                    Homework = homework,
                    Students = new List<DAL.University.Identity.User>() { user }
                });

                context.SaveChanges();
            }

            using (var context = new UniversityContext(_options))
            {
                var mapper = new Mock<IMapper>();
                var notifyService = new Mock<IEmailService>();
                var smsService = new Mock<ISmsService>();
                var studyService = new StudyService(usersRepository.Object, lecturesRepository.Object, logger.Object,
                    context, mapper.Object, notifyService.Object, smsService.Object);
                Assert.That(() => studyService.AddStudentToLecture(guidUser, guid, It.IsAny<bool>()), Throws.Exception.TypeOf<UserAlreadyExistException>());
            }

        }
        [Test]
        public void AddStudentToLecture_AllConditionAreOk_Return_True()
        {
            var usersRepository = new Mock<IRepository<User>>();

            var lecturesRepository = new Mock<IRepository<Lecture>>();
            var homework = new DAL.University.Entities.Homework()
            {
                Id = Guid.NewGuid()
            };
            var logger = new Mock<ILogger<StudyService>>();
            var guid = Guid.NewGuid();
            var guidUser = Guid.NewGuid();
            var user = new DAL.University.Identity.User()
            {
                Id = guidUser,
                UserName = "Denis",
                Role = "student"
            };
            var lector = new DAL.University.Identity.User()
            {
                Id = Guid.NewGuid(),
                UserName = "Lector",
                Role = "lector"
            };
            using (var context = new UniversityContext(_options))
            {
                context.Users.Add(user);
                context.Lectures.Add(new()
                {
                    Id = guid,
                    LectureName = "Lecture",
                    Homework = homework,
                    Students = new List<DAL.University.Identity.User>() { lector }
                });

                context.SaveChanges();
            }

            using (var context = new UniversityContext(_options))
            {
                var mapper = new Mock<IMapper>();
                var notifyService = new Mock<IEmailService>();
                var smsService = new Mock<ISmsService>();
                var studyService = new StudyService(usersRepository.Object, lecturesRepository.Object, logger.Object,
                    context, mapper.Object, notifyService.Object, smsService.Object);
                Assert.That(() => studyService.AddStudentToLecture(guidUser, guid, It.IsAny<bool>()), Is.True);
                Assert.That(() => context.Journal.ToList(), Is.Not.Null);
            }

        }

        [Test]
        public void GetAttendanceByLecture_LectureIsNull_Throws_LectureNotExistException()
        {
            var usersRepository = new Mock<IRepository<User>>();

            var lecturesRepository = new Mock<IRepository<Lecture>>();

            var logger = new Mock<ILogger<StudyService>>();
            var guidUser = Guid.NewGuid();
            var user = new DAL.University.Identity.User()
            {
                Id = guidUser,
                UserName = "Denis",
                Role = "student"
            };

            using (var context = new UniversityContext(_options))
            {
                context.Users.Add(user);

                context.SaveChanges();
            }

            using (var context = new UniversityContext(_options))
            {
                var mapper = new Mock<IMapper>();
                var notifyService = new Mock<IEmailService>();
                var smsService = new Mock<ISmsService>();
                var studyService = new StudyService(usersRepository.Object, lecturesRepository.Object, logger.Object,
                    context, mapper.Object, notifyService.Object, smsService.Object);
                Assert.That(() => studyService.GetAttendanceByLecture(It.IsAny<string>()), Throws.Exception.TypeOf<LectureNotExistException>());
            }
        }
        [Test]
        public void GetAttendanceByLecture_JournalAreEmpty_Throws_JournalNotExistException()
        {
            var usersRepository = new Mock<IRepository<User>>();

            var lecturesRepository = new Mock<IRepository<Lecture>>();
            
            var logger = new Mock<ILogger<StudyService>>();
            var guid = Guid.NewGuid();
            var guidUser = Guid.NewGuid();
            
            using (var context = new UniversityContext(_options))
            {
                context.Lectures.Add(new()
                {
                    Id = guid,
                    LectureName = "Lecture",
                });

                context.SaveChanges();
            }

            using (var context = new UniversityContext(_options))
            {
                var mapper = new Mock<IMapper>();
                var notifyService = new Mock<IEmailService>();
                var smsService = new Mock<ISmsService>();
                var studyService = new StudyService(usersRepository.Object, lecturesRepository.Object, logger.Object,
                    context, mapper.Object, notifyService.Object, smsService.Object);
                Assert.That(() => studyService.GetAttendanceByLecture("Lecture"), Throws.Exception.TypeOf<JournalNotExistException>());
                //Assert.That(() => context.Journal.ToList().Count, Is.EqualTo(0));
            }

        }
        [Test]
        public void GetAttendanceByLecture_JournalIsNotNull_Returns_Journal()
        {
            // TODO: надо разобраться с этим и предыдущим тестом, возникает конфликт (06.07.2021 13:53 )
            var usersRepository = new Mock<IRepository<User>>();

            var lecturesRepository = new Mock<IRepository<Lecture>>();
            var homework = new DAL.University.Entities.Homework()
            {
                Id = Guid.NewGuid()
            };
            var logger = new Mock<ILogger<StudyService>>();
            var guid = Guid.NewGuid();
            var guidUser = Guid.NewGuid();
            var user = new DAL.University.Identity.User()
            {
                Id = guidUser,
                UserName = "Denis",
                Role = "student"
            };
            var lector = new DAL.University.Identity.User()
            {
                Id = Guid.NewGuid(),
                UserName = "Lector",
                Role = "lector"
            };
            using (var context = new UniversityContext(_options))
            {
                context.Users.AddRange(lector, user);
                context.Lectures.Add(new()
                {
                    Id = guid,
                    LectureName = "Lecture",
                    Homework = homework,
                    Students = new List<DAL.University.Identity.User>() { lector, user }
                });
                context.Journal.Add(new Journal()
                {
                    Id = Guid.NewGuid(),
                    LectureId = guid,
                    StudentId = guidUser
                });

                context.SaveChanges();
            }

            using (var context = new UniversityContext(_options))
            {
                var mapper = new Mock<IMapper>();
                var notifyService = new Mock<IEmailService>();
                var smsService = new Mock<ISmsService>();
                var studyService = new StudyService(usersRepository.Object, lecturesRepository.Object, logger.Object,
                    context, mapper.Object, notifyService.Object, smsService.Object);
                var result = studyService.GetAttendanceByLecture("Lecture");
                Assert.That(() => context.Journal.ToList().Count, Is.EqualTo(1));
            }

        }

        [Test]
        public void GetSkipLectureCount_UserIsNull_Throws_UserNotExistException()
        {
            var usersRepository = new Mock<IRepository<User>>();

            var lecturesRepository = new Mock<IRepository<Lecture>>();
            var homework = new DAL.University.Entities.Homework()
            {
                Id = Guid.NewGuid()
            };
            var logger = new Mock<ILogger<StudyService>>();
            var guid = Guid.NewGuid();
            var guidUser = Guid.NewGuid();
            var user = new DAL.University.Identity.User()
            {
                Id = guidUser,
                UserName = "Denis",
                Role = "student"
            };
            var lector = new DAL.University.Identity.User()
            {
                Id = Guid.NewGuid(),
                UserName = "Lector",
                Role = "lector"
            };
            using (var context = new UniversityContext(_options))
            {
                context.Users.AddRange(lector, user);
                context.Lectures.Add(new()
                {
                    Id = guid,
                    LectureName = "Lecture",
                    Homework = homework,
                    Students = new List<DAL.University.Identity.User>() { lector, user }
                });
                context.Journal.Add(new Journal()
                {
                    Id = Guid.NewGuid(),
                    LectureId = guid,
                    StudentId = guidUser
                });

                context.SaveChanges();
            }

            using (var context = new UniversityContext(_options))
            {
                var mapper = new Mock<IMapper>();
                var notifyService = new Mock<IEmailService>();
                var smsService = new Mock<ISmsService>();
                var studyService = new StudyService(usersRepository.Object, lecturesRepository.Object, logger.Object,
                    context, mapper.Object, notifyService.Object, smsService.Object);
                Assert.That(() => studyService.GetSkipLectureCount(Guid.NewGuid()), Throws.Exception.TypeOf<UserNotExistException>());

            }
        }
        [Test]
        public void GetSkipLectureCount_AllConditionAreOk_Return_Count()
        {
            var usersRepository = new Mock<IRepository<User>>();
            var homework = new DL.Models.Homework
            {
                Id = Guid.NewGuid()
            };
            var guid = Guid.NewGuid();
            var guidUser = Guid.NewGuid();
            var user = new DAL.University.Identity.User()
            {
                Id = guidUser,
                UserName = "Denis",
                Role = "student",
                Lectures = new List<DAL.University.Entities.Lecture>()
            };
            var lector = new User()
            {
                Id = Guid.NewGuid(),
                Username = "Lector",
                Role = "lector"
            };
            var lecture = new Lecture()
            {
                Id = guid,
                LectureName = "Lecture",
                Homework = homework,
                Students = new List<User>() { lector }
            };
            var listLectures = new List<Lecture>();
            listLectures.Add(lecture);
            var lecturesRepository = new Mock<IRepository<Lecture>>();
            lecturesRepository.Setup(x => x.GetAll()).Returns(listLectures);

            var logger = new Mock<ILogger<StudyService>>();

            using (var context = new UniversityContext(_options))
            {
                context.Users.AddRange(user);

                context.SaveChanges();
            }

            using (var context = new UniversityContext(_options))
            {
                var mapper = new Mock<IMapper>();
                var notifyService = new Mock<IEmailService>();
                var smsService = new Mock<ISmsService>();
                var studyService = new StudyService(usersRepository.Object, lecturesRepository.Object, logger.Object,
                    context, mapper.Object, notifyService.Object, smsService.Object);
                Assert.That(() => studyService.GetSkipLectureCount(guidUser), Is.EqualTo(1));
            }
        }

        [Test]
        public void GetAverageMark_UserIsNull_Throws_UserNotExistException()
        {
            var usersRepository = new Mock<IRepository<User>>();
            var lecturesRepository = new Mock<IRepository<Lecture>>();
            var logger = new Mock<ILogger<StudyService>>();
            using (var context = new UniversityContext(_options))
            {
                context.Users.AddRange();
                context.SaveChanges();
            }
            using (var context = new UniversityContext(_options))
            {
                var mapper = new Mock<IMapper>();
                var notifyService = new Mock<IEmailService>();
                var smsService = new Mock<ISmsService>();
                var studyService = new StudyService(usersRepository.Object, lecturesRepository.Object, logger.Object,
                    context, mapper.Object, notifyService.Object, smsService.Object);
                Assert.That(() => studyService.GetAverageMark(It.IsAny<Guid>()), Throws.Exception.TypeOf<UserNotExistException>());
            }
        }
        [Test]
        public void GetAverageMark_UserIsNotNull_Return_AverageMark()
        {
            // TODO: доделать (06.07.2021 16:44 )

            var usersRepository = new Mock<IRepository<User>>();
            var logger = new Mock<ILogger<StudyService>>();

            var guidUser = Guid.NewGuid();
            var guidLector = Guid.NewGuid();
            var guidLecture = Guid.NewGuid();
            var lector = new DAL.University.Identity.User()
            {
                Id = guidLector,
                UserName = "Lector"
            };
            var user = new DAL.University.Identity.User()
            {
                Id = guidUser,
                UserName = "Denis",
                Role = "student",
                Lectures = new List<DAL.University.Entities.Lecture>()
            };
            var lecture = new DAL.University.Entities.Lecture()
            {
                Id = guidLecture,
                Students = new List<DAL.University.Identity.User>() {  }
            };
            var lectureRep = new Lecture()
            {
                Id = guidLecture,
                Students = new List<User>() {  }
            };
            var lecturesRepository = new Mock<IRepository<Lecture>>();
            lecturesRepository.Setup(x => x.GetAll()).Returns(new List<Lecture>() {lectureRep});

            using (var context = new UniversityContext(_options))
            {
                context.Users.AddRange(lector, user);
                context.Journal.Add(new()
                {
                    Id = Guid.NewGuid(),
                    LectureId = guidLecture,
                    Lecture = lecture,
                    StudentId = guidUser,
                    Student = user,
                    HasHomework = true,
                    IsAttended = true,
                    Mark = 5
                });
                context.SaveChanges();
            }
            using (var context = new UniversityContext(_options))
            {
                var mapperConfig = new MapperConfiguration(config =>
                {
                    config.AddProfile<BusinessLogicMapperProfile>();
                });
                var map = mapperConfig.CreateMapper();
                var notifyService = new Mock<IEmailService>();
                var smsService = new Mock<ISmsService>();
                var studyService = new StudyService(usersRepository.Object, lecturesRepository.Object, logger.Object,
                    context, map, notifyService.Object, smsService.Object);

                Assert.That(() => studyService.GetAverageMark(guidUser), Is.EqualTo(2.5));
            }
        }

    }

}
