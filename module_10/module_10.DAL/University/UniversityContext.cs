using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using module_10.DAL.University.Entities;
using module_10.DAL.University.Identity;

namespace module_10.DAL.University
{
    public class UniversityContext : IdentityDbContext<User, Role, Guid>
    {
        public UniversityContext()
        {
        }
        public DbSet<Journal> Journal { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Homework> Homeworks { get; set; }
        public UniversityContext(DbContextOptions<UniversityContext> options)
        : base(options)
        {

        }

    }
}
