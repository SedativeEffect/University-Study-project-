using Microsoft.Extensions.DependencyInjection;
using module_10.BLL.Services;
using module_10.DL.Interfaces;
using module_10.DL.Models;

namespace module_10.BLL.Utils
{
    public static class BusinessLogicRegistrator
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            services
                .AddAutoMapper(typeof(BusinessLogicMapperProfile))
                .AddScoped<IStudentService<User>, StudentService>()
                .AddScoped<ILectureService<Lecture>, LectureService>()
                .AddScoped<IHomeworkService<Homework>, HomeworkService>()
                .AddScoped<IStudyService<Journal>, StudyService>()
                .AddScoped<IEmailService, EmailService>()
                .AddScoped<ISmsService, SmsService>();

            return services;
        }

    }

}
