using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using module_10.BLL.Exceptions;
using module_10.BLL.Utils;
using module_10.DL.Interfaces;
using module_10.DL.Models;

namespace module_10.BLL.Services
{
    public class HomeworkService : IHomeworkService<Homework>
    {
        private readonly IRepository<Homework> _homeworkRepository;
        private readonly ILogger<HomeworkService> _logger;
        private readonly ILectureService<Lecture> _lectureService;

        public HomeworkService(IRepository<Homework> homeworkRepository, ILogger<HomeworkService> logger, ILectureService<Lecture> lectureService)
        {
            _homeworkRepository = homeworkRepository;
            _logger = logger;
            _lectureService = lectureService;
        }
        public IEnumerable<Homework> GetAllHomeworks()
        {
            return _homeworkRepository.GetAll();
        }

        public Homework Get(Guid id)
        {
            var homework = _homeworkRepository.Get(id);
            if (homework is null)
            {
                _logger.LogAndThrow(new HomeworkNotExistException($"Homework with id {id} not exist"), LogLevel.Warning);
            }
            return homework;
        }

        public Homework Create(Homework homework)
        {
            var lecture = _lectureService.Get(homework.LectureId);
            if (lecture is null)
            {
                _logger.LogAndThrow(new LectureNotExistException($"Lecture with id {homework.LectureId} not exist"), LogLevel.Warning);
            }

            if (lecture?.Homework is not null)
            {
                _logger.LogAndThrow(new HomeworkAlreadyExistException($"Lecture with id {lecture.Id} already have homework"), LogLevel.Warning);
            }
            var homeworkResult = _homeworkRepository.Create(homework);
            
            return homeworkResult;
        }

        public Homework Update(Guid id, Homework homework)
        {
            homework.Id = id;
            var homeworkResult = _homeworkRepository.Update(homework);
            if (homeworkResult is null)
            {
                _logger.LogAndThrow(new HomeworkNotExistException($"Homework with id {id} not exist"), LogLevel.Warning);
            }
            return homeworkResult;
        }

        public bool Delete(Guid id)
        {
            var isDelete = _homeworkRepository.Delete(id);
            if (!isDelete)
            {
                _logger.LogAndThrow(new HomeworkNotExistException($"Homework with id {id} not exist"), LogLevel.Warning);
            }
            return isDelete;
        }
    }
}
