using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using module_10.DAL.University.Entities;
using module_10.DAL.University.Identity;

namespace module_10.DAL.University.Utils
{
    public static class DatabaseInitializer
    {
        public static void InitDatabase(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetService<ILogger<DbLoggerCategory.Database>>();

            try
            {
                logger.LogDebug("Start seeding DB!");

                var dbContext = serviceProvider.GetService<UniversityContext>();

                dbContext?.Database.EnsureDeleted();
                dbContext?.Database.Migrate();

                var userManager = serviceProvider.GetService<UserManager<User>>();
                var users = new List<User>();
                users.AddRange(new User[]
                {
                new()
                {
                    UserName = "SedativeEffect",
                    Email = "denis.kozak@epam.com",
                    EmailConfirmed = true,
                    Role = "student",
                    Lectures = new List<Lecture>(),
                    Homeworks = new List<Homework>()
                },
                new()
                {
                    UserName = "Vova",
                    Email = "vova.petrov@epam.com",
                    EmailConfirmed = true,
                    Role = "student",
                    Lectures = new List<Lecture>(),
                    Homeworks = new List<Homework>()
                },
                new()
                {
                    UserName = "Dmitriy",
                    Email = "dmitriy.belov@epam.com",
                    EmailConfirmed = true,
                    Role = "student",
                    Lectures = new List<Lecture>(),
                    Homeworks = new List<Homework>()
                }
                });


                foreach (var user in users)
                {
                    var result = userManager?.CreateAsync(user, "password").GetAwaiter().GetResult();
                    if (result is not null && result.Succeeded)
                    {
                        userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "student")).GetAwaiter().GetResult();
                    }
                }
                var lector = new User
                {
                    UserName = "Lector",
                    Email = "lector.lector@epam.com",
                    EmailConfirmed = true,
                    Role = "lector",
                    Lectures = new List<Lecture>(),
                    Homeworks = new List<Homework>()
                };

                var resultLector = userManager?.CreateAsync(lector, "password").GetAwaiter().GetResult();
                if (resultLector is not null && resultLector.Succeeded)
                {
                    userManager.AddClaimAsync(lector, new Claim(ClaimTypes.Role, "lector")).GetAwaiter().GetResult();
                }
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Failed to seed DB!");
                throw;
            }


        }
    }
}
