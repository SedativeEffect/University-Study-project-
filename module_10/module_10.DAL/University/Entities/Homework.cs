using System;

namespace module_10.DAL.University.Entities
{
    public class Homework
    {
        public Guid Id { get; set; }
        public Guid LectureId { get; set; }
        public Lecture Lecture { get; set; }
        public string Task { get; set; }
    }
}
