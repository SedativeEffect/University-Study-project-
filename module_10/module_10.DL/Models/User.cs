using System;
using System.Collections.Generic;

namespace module_10.DL.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public string PhoneNumber { get; set; }
        public IEnumerable<Lecture> Lectures { get; set; }
        public IEnumerable<Homework> Homeworks { get; set; }
    }
}
