using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using module_10.DAL.University.Entities;

namespace module_10.DAL.University.Identity
{
    public class User : IdentityUser<Guid>
    {
        public string Role { get; set; }
        public IEnumerable<Lecture> Lectures { get; set; }
        public IEnumerable<Homework> Homeworks { get; set; } = new List<Homework>();

    }
}
