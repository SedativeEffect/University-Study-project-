using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using module_10.DAL.University.Entities;
using module_10.DL.Interfaces;
using Domain = module_10.DL.Models;
namespace module_10.DAL.University.Repositories
{
    internal class LecturesRepository : IRepository<Domain.Lecture>
    {
        private readonly UniversityContext _dbContext;
        private readonly IMapper _mapper;

        public LecturesRepository(UniversityContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public IEnumerable<Domain.Lecture> GetAll()
        {
            var dbLectures = _dbContext.Lectures
                .Include(x => x.Homework)
                .Include(x => x.Students)
                .ToList();
            return _mapper.Map<IEnumerable<Domain.Lecture>>(dbLectures);
        }

        public Domain.Lecture Get(Guid id)
        {
            var dbLecture = _dbContext.Lectures
                .Include(x => x.Homework)
                .Include(x => x.Students)
                .FirstOrDefault(x => x.Id == id);
            return dbLecture is null ? null : _mapper.Map<Domain.Lecture>(dbLecture);
        }

        public Domain.Lecture Create(Domain.Lecture entity)
        {
            var dbLecture = _mapper.Map<Lecture>(entity);
            var result = _dbContext.Lectures.Add(dbLecture);
            if (result.State != EntityState.Added)
            {
                return null;
            }
            _dbContext.SaveChanges();
            return _mapper.Map<Domain.Lecture>(result.Entity);
        }

        public Domain.Lecture Update(Domain.Lecture entity)
        {
            var findLecture = _dbContext.Lectures
                .Include(x => x.Homework)
                .Include(x => x.Students)
                .FirstOrDefault(x => x.Id == entity.Id);
            if (findLecture is Lecture dbLecture)
            {
                //_mapper.Map(entity, dbLecture);
                dbLecture.LectureDate = entity.LectureDate;
                dbLecture.Subject = entity.Subject;
                _dbContext.Entry(dbLecture).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return _mapper.Map<Domain.Lecture>(dbLecture);
            }

            return null;
        }

        public bool Delete(Guid id)
        {
            if (_dbContext.Lectures.Find(id) is Lecture dbLecture)
            {
                _dbContext.Entry(dbLecture).State = EntityState.Deleted;
                _dbContext.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
