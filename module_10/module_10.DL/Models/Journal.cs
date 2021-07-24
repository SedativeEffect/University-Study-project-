using System;

namespace module_10.DL.Models
{
    public class Journal
    {
        public Guid Id { get; set; }
        public Guid LectureId { get; set; }
        public Lecture Lecture { get; set; }

        public Guid StudentId { get; set; }

        public User Student { get; set; }

        public int Mark { get; set; }

        public bool HasHomework { get; set; }
        public bool IsAttended { get; set; }

    }
}
