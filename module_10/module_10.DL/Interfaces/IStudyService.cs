using System;
using System.Collections.Generic;

namespace module_10.DL.Interfaces
{
    public interface IStudyService<T> where T : class
    {
        bool AddStudentToLecture(Guid studentId, Guid lectureId, bool hasHomework);

        IEnumerable<T> GetAttendanceByLecture(string lectureName);

        IEnumerable<T> GetAttendanceByStudent(string studentName);

        int GetSkipLectureCount(Guid studentId);

        double GetAverageMark(Guid studentId);

    }
}
