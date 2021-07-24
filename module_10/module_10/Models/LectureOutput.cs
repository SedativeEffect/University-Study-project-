using System;
using System.Collections.Generic;

namespace module_10.Models
{
    public class LectureOutput
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string LectureName { get; set; }
        public DateTime LectureDate { get; set; }
        public HomeworkOutput Homework { get; set; }
        public IEnumerable<UserOutput> Students { get; set; }
    }
}
