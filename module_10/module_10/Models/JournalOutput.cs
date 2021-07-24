using System;

namespace module_10.Models
{
    public class JournalOutput
    {
        public string LectureName { get; set; }
        public DateTime LectureDate { get; set; }
        public string UserName { get; set; }
        public int Mark { get; set; }
        public bool HasHomework { get; set; }
        public bool IsAttended { get; set; }
    }
}
