using System;

namespace module_10.Models
{
    public class LectureInput
    {
        public int Subject { get; set; }
        public string LectureName { get; set; }
        public HomeworkInput Homework { get; set; }
        public DateTime LectureDate { get; set; }
    }
}
