using System;
using System.Collections.Generic;

namespace module_10.DL.Interfaces
{
    public interface ILectureService<T> where T : class
    {
        IEnumerable<T> GetAllLectures();

        T Get(Guid id);

        T Create(T lecture);

        T Update(Guid id, T lecture);

        bool Delete(Guid id);
    }
}
