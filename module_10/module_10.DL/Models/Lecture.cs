using System;
using System.Collections.Generic;

namespace module_10.DL.Models
{
    public class Lecture
    {
        public Guid Id { get; set; }

        public string LectureName { get; set; }
        public int Subject { get; set; }
        public DateTime LectureDate { get; set; }
        public Homework Homework { get; set; }
        public IEnumerable<User> Students { get; set; }
        public override bool Equals(object obj)
        {
            Lecture l = obj as Lecture;
            if (l == null) return false;

            return Id == l.Id;
        }
    }
}
