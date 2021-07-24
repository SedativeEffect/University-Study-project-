using System;
using System.Collections.Generic;

namespace module_10.DL.Interfaces
{
    public interface IStudentService<T> where T : class 
    {
        IEnumerable<T> GetAllStudents();

        T Get(Guid id);

        T Create(T student);

        T Update(Guid id, T student);

        bool Delete(Guid id);

        
    }
}
