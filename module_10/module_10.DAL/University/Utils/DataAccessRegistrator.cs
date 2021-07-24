using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using module_10.DAL.University.Identity;
using module_10.DAL.University.Repositories;
using module_10.DL.Interfaces;
using Domain = module_10.DL.Models;
namespace module_10.DAL.University.Utils
{
    public static class DataAccessRegistrator
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, string connectionString)
        {
            services
                .AddScoped<IRepository<Domain.User>, StudentsRepository>()
                .AddScoped<IRepository<Domain.Lecture>, LecturesRepository>()
                .AddScoped<IRepository<Domain.Homework>, HomeworksRepository>()
                .AddDbContext<UniversityContext>(options => options.UseSqlServer(connectionString))
                .AddAutoMapper(typeof(DataAccessMapperProfile))
                .AddIdentity<User, Role>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<UniversityContext>();

            return services;
        }
    }
}
