using System;

namespace module_10.Models
{
    public class UserOutput
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        //public IEnumerable<LectureOutput> Lectures { get; set; }
        //public IEnumerable<HomeworkOutput> Homeworks { get; set; }
    }
}
