using System;
using System.Collections.Generic;
using module_10.DAL.University.Identity;

namespace module_10.DAL.University.Entities
{
    public class Lecture
    {
        public Guid Id { get; set; }
        public int Subject { get; set; }
        public string LectureName { get; set; }
        public DateTime LectureDate { get; set; }
        public Homework Homework { get; set; }
        public IEnumerable<User> Students { get; set; } = new List<User>();
    }
}
