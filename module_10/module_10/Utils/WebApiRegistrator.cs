using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using module_10.BLL.Models;
using module_10.Models;
using module_10.Utils.Validation;

namespace module_10.Utils
{
    public static class WebApiRegistrator
    {
        public static IServiceCollection AddWebApi(this IServiceCollection services)
        {
            services
                .AddAutoMapper(typeof(WebApiMapperProfile))
                .AddTransient<IValidator<UserInput>, UserValidator>()
                .AddTransient<IValidator<LectureInput>, LectureValidator>()
                .AddTransient<IValidator<LectureToUpdate>, LectureUpdateValidator>()
                .AddControllers()
                .AddXmlDataContractSerializerFormatters();


            return services;
        }

    }
}
