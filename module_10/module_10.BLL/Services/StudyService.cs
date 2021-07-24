using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using module_10.BLL.Exceptions;
using module_10.BLL.Utils;
using module_10.DAL.University;
using module_10.DL.Interfaces;
using Domain = module_10.DL.Models;
using module_10.DAL.University.Entities;
namespace module_10.BLL.Services
{
    public class StudyService : IStudyService<Domain.Journal>
    {
        private readonly IRepository<Domain.User> _usersRepository;
        private readonly IRepository<Domain.Lecture> _lecturesRepository;
        private readonly ILogger<StudyService> _logger;
        private readonly UniversityContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;

        public StudyService(IRepository<Domain.User> usersRepository
            , IRepository<Domain.Lecture> lecturesRepository
            , ILogger<StudyService> logger
            , UniversityContext dbContext
            , IMapper mapper
            , IEmailService emailService
            , ISmsService smsService)
        {
            _usersRepository = usersRepository;
            _lecturesRepository = lecturesRepository;
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
            _emailService = emailService;
            _smsService = smsService;
        }
        public bool AddStudentToLecture(Guid studentId, Guid lectureId, bool hasHomework)
        {
            var lectureDb = _dbContext.Lectures
                .Include(x => x.Students)
                .Include(x => x.Homework)
                .FirstOrDefault(x => x.Id == lectureId);

            if (lectureDb is null)
            {
                _logger.LogAndThrow(new LectureNotExistException($"Lecture with id {lectureId} not exist"), LogLevel.Warning);
            }

            if (lectureDb?.Homework is null)
            {
                _logger.LogAndThrow(new HomeworkNotExistException($"Before enrolling student for a lecture, add lecture homework"), LogLevel.Warning);
            }

            var user = _dbContext.Users.FirstOrDefault(x => x.Id == studentId);

            if (user is null)
            {
                _logger.LogAndThrow(new UserNotExistException($"User with id {studentId} not exist"), LogLevel.Warning);
            }

            var students = lectureDb.Students.ToList();
            if (students.Contains(user))
            {
                _logger.LogAndThrow(new UserAlreadyExistException($"User with id {studentId} already attends the lecture"), LogLevel.Warning);
            }

            if (user.Role == "lector" && students.Contains(user))
            {
                _logger.LogAndThrow(new UserAlreadyExistException("There can be 1 teacher per lecture"), LogLevel.Warning);
            }

            if (!students.Any(x => x.Role == "lector") && user.Role == "student")
            {
                _logger.LogAndThrow(new NoLectorAtLectureException("Need add lector to lecture"), LogLevel.Warning);
            }


            if (hasHomework)
            {
                var userHomeworks = user.Homeworks.ToList();
                userHomeworks.Add(lectureDb.Homework);
                user.Homeworks = userHomeworks;
            }

            students.Add(user);
            lectureDb.Students = students;

            var rnd = new Random();

            _dbContext.Journal.Add(new Journal
            {
                HasHomework = hasHomework,
                IsAttended = true,
                Lecture = lectureDb,
                Mark = hasHomework ? rnd.Next(1, 5) : 0,
                Student = user
            });

            _dbContext.SaveChanges();
            return true;

        }

        public IEnumerable<Domain.Journal> GetAttendanceByLecture(string lectureName)
        {
            var lectureDb = _dbContext.Lectures.FirstOrDefault(x => x.LectureName == lectureName);
            if (lectureDb is null)
            {
                _logger.LogAndThrow(new LectureNotExistException($"Lecture with name {lectureName} not exist"), LogLevel.Warning);
            }

            var journal = _dbContext.Journal
                .Include(x => x.Student)
                .Include(x => x.Lecture)
                .AsNoTracking()
                .Where(x => x.LectureId == lectureDb.Id && x.Student.Role != "lector")
                .ToList();

            if (journal.Count == 0)
            {
                _logger.LogAndThrow(new JournalNotExistException("There is no information about this lecture in the journal"), LogLevel.Warning);
            }

            var allUsers = _usersRepository.GetAll();
            foreach (var user in allUsers)
            {
                var userLectures = user.Lectures.ToList();
                if (!userLectures.Contains(_mapper.Map<Domain.Lecture>(lectureDb)))
                {
                    journal.Add(new()
                    {
                        LectureId = lectureDb.Id,
                        HasHomework = false,
                        IsAttended = false,
                        Lecture = lectureDb,
                        Mark = 0,
                        Student = _mapper.Map<DAL.University.Identity.User>(user),
                        StudentId = user.Id
                    });
                }
            }

            return _mapper.Map<IEnumerable<Domain.Journal>>(journal);
        }

        public IEnumerable<Domain.Journal> GetAttendanceByStudent(string studentName)
        {
            var user = _dbContext.Users
                .Include(x => x.Homeworks)
                .Include(x => x.Lectures)
                .FirstOrDefault(x => x.UserName == studentName);

            if (user is null)
            {
                _logger.LogAndThrow(new UserNotExistException($"User with name {studentName} not exist"), LogLevel.Warning);
            }

            if (user.Role == "lector")
            {
                _logger.LogAndThrow(new AttendanceException("You can't get attendance by lector"), LogLevel.Warning);
            }

            var journal = _dbContext.Journal
                .Include(x => x.Student)
                .Include(x => x.Lecture)
                .AsNoTracking()
                .Where(x => x.StudentId == user.Id && x.Student.Role != "lector")
                .ToList();
            if (journal.Count == 0)
            {
                _logger.LogAndThrow(new JournalNotExistException("There is no information about this lecture in the journal"), LogLevel.Warning);
            }
            var allLectures = _lecturesRepository.GetAll();
            var userLectures = user.Lectures.ToList();

            foreach (var lecture in allLectures)
            {
                if (!userLectures.Any(x => x.Id == lecture.Id))
                {
                    journal.Add(new()
                    {
                        LectureId = lecture.Id,
                        HasHomework = false,
                        IsAttended = false,
                        Lecture = _mapper.Map<Lecture>(lecture),
                        Mark = 0,
                        Student = _mapper.Map<DAL.University.Identity.User>(user),
                        StudentId = user.Id
                    });
                }
            }
            
            return _mapper.Map<IEnumerable<Domain.Journal>>(journal);

        }

        public int GetSkipLectureCount(Guid studentId)
        {
            var user = _dbContext.Users
                .Include(x => x.Homeworks)
                .Include(x => x.Lectures)
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == studentId);

            if (user is null)
            {
                _logger.LogAndThrow(new UserNotExistException($"User with id {studentId} not exist"), LogLevel.Warning);
            }

            var allLectures = _lecturesRepository.GetAll();

            var skipLectureCount = allLectures.Count() - user.Lectures.Count();
            if (skipLectureCount > 3)
            {
                var lectorEmails = _dbContext.Users.AsNoTracking().Where(x => x.Role == "lector").ToList().Select(x => x.Email);
                _emailService.NotifyByEmail(string.Join(",", lectorEmails), "Attendance", $"Student {user.UserName} missed more than 4 lectures")
                    .GetAwaiter().GetResult();
            }

            return skipLectureCount;
        }

        public double GetAverageMark(Guid studentId)
        {
            var user = _dbContext.Users
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == studentId);

            if (user is null)
            {
                _logger.LogAndThrow(new UserNotExistException($"User with id {studentId} not exist"), LogLevel.Warning);
            }

            var journal = GetAttendanceByStudent(user.UserName);

            var groupMarks = journal.GroupBy(x => x.Mark).Select(g => new
            {
                Mark = g.Key,
                Count = g.Count()
            });

            double result = 0.0;
            foreach (var groupMark in groupMarks)
            {
                result += groupMark.Mark * groupMark.Count;
            }

            result /= groupMarks.Select(x => x.Count).Sum();

            if (result < 4)
            {
                _smsService.NotifyBySms(user.PhoneNumber, "Your average grade for the course is below 4 points")
                    .GetAwaiter().GetResult();
            }

            return Math.Round(result, 2);
        }

    }
}
