using System;
using System.Collections.Generic;

namespace module_10.DL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(Guid id);

        T Create(T entity);

        T Update(T entity);

        bool Delete(Guid id);


    }
}
