using System;
using System.Collections.Generic;

namespace module_10.DL.Interfaces
{
    public interface IHomeworkService<T> where T : class
    {
        IEnumerable<T> GetAllHomeworks();

        T Get(Guid id);

        T Create(T homework);

        T Update(Guid id, T homework);

        bool Delete(Guid id);
    }
}
