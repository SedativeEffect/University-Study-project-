using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using module_10.BLL.Exceptions;
using module_10.BLL.Utils;
using module_10.DAL.University;
using module_10.DL.Interfaces;
using module_10.DL.Models;

namespace module_10.BLL.Services
{
    public class LectureService : ILectureService<Lecture>
    {
        private readonly IRepository<Lecture> _lecturesRepository;
        private readonly ILogger<LectureService> _logger;

        public LectureService(IRepository<Lecture> lecturesRepository, ILogger<LectureService> logger)
        {
            _lecturesRepository = lecturesRepository;
            _logger = logger;
        }
        public IEnumerable<Lecture> GetAllLectures()
        {
            return _lecturesRepository.GetAll();
        }

        public Lecture Get(Guid id)
        {
            var lecture = _lecturesRepository.Get(id);
            if (lecture is null)
            {
                _logger.LogAndThrow(new LectureNotExistException($"Lecture with id {id} not exist"), LogLevel.Warning);
            }
            return lecture;
        }

        public Lecture Create(Lecture lecture)
        {
            var lectures = _lecturesRepository.GetAll();

            if (lectures.Any(x => x.LectureName == lecture.LectureName))
            {
                _logger.LogAndThrow(new LectureAlreadyExistException($"Lecture with Lecture Name {lecture.LectureName} already exist"), LogLevel.Warning);
            }
            if (lecture.Homework is not null)
            {
                lecture.Homework.Lecture = lecture;
            }
            var lectureResult = _lecturesRepository.Create(lecture);

            return lectureResult;
        }

        public Lecture Update(Guid id, Lecture lecture)
        {
            lecture.Id = id;
            var lectureResult = _lecturesRepository.Update(lecture);
            if (lectureResult is null)
            {
                _logger.LogAndThrow(new LectureNotExistException($"Lecture with id {id} not exist"), LogLevel.Warning);
            }
            return lectureResult;
        }

        public bool Delete(Guid id)
        {
            var isDelete = _lecturesRepository.Delete(id);
            if (!isDelete)
            {
                _logger.LogAndThrow(new LectureNotExistException($"Lecture with id {id} not exist"), LogLevel.Warning);
            }
            return isDelete;
        }
    }
}
