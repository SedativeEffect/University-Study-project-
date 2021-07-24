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
    internal class HomeworksRepository : IRepository<Domain.Homework>
    {
        private readonly UniversityContext _dbContext;
        private readonly IMapper _mapper;

        public HomeworksRepository(UniversityContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public IEnumerable<Domain.Homework> GetAll()
        {
            var dbHomeworks = _dbContext.Homeworks
                .Include(x => x.Lecture)
                .ToList();
            return _mapper.Map<IEnumerable<Domain.Homework>>(dbHomeworks);
        }

        public Domain.Homework Get(Guid id)
        {
            var dbHomework = _dbContext.Homeworks
                .Include(x => x.Lecture)
                .FirstOrDefault(x => x.Id == id);
            return dbHomework is null ? null : _mapper.Map<Domain.Homework>(dbHomework);
        }

        public Domain.Homework Create(Domain.Homework entity)
        {
            var dbHomework = _mapper.Map<Homework>(entity);
            var result = _dbContext.Homeworks.Add(dbHomework);
            if (result.State != EntityState.Added)
            {
                return null;
            }
            _dbContext.SaveChanges();
            return _mapper.Map<Domain.Homework>(result.Entity);
        }

        public Domain.Homework Update(Domain.Homework entity)
        {
            var findHomework = _dbContext.Homeworks
                .Include(x => x.Lecture)
                .FirstOrDefault(x => x.Id == entity.Id);
            if (findHomework is Homework dbHomework)
            {
                dbHomework.Task = entity.Task;
                _dbContext.Entry(dbHomework).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return _mapper.Map<Domain.Homework>(dbHomework);
            }

            return null;
        }

        public bool Delete(Guid id)
        {
            if (_dbContext.Homeworks.Find(id) is Homework dbHomework)
            {
                _dbContext.Entry(dbHomework).State = EntityState.Deleted;
                _dbContext.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
