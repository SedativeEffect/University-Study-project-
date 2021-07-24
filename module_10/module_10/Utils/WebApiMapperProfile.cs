using System;
using AutoMapper;
using module_10.BLL.Enums;
using module_10.Models;
using Domain = module_10.DL.Models;
namespace module_10.Utils
{
    internal class WebApiMapperProfile : Profile
    {
        public WebApiMapperProfile()
        {
            CreateMap<UserInput, Domain.User>()
                .ReverseMap();

            CreateMap<LectureInput, Domain.Lecture>()
                .ReverseMap();

            CreateMap<HomeworkToLectureInput, Domain.Homework>()
                .ReverseMap();

            CreateMap<HomeworkInput, Domain.Homework>()
                .ReverseMap();

            CreateMap<Domain.Lecture, LectureOutput>()
                .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => Enum.GetName(typeof(Subject), src.Subject)))
                .ReverseMap();

            CreateMap<HomeworkOutput, Domain.Homework>()
                .ReverseMap();

            CreateMap<UserOutput, Domain.User>()
                .ReverseMap();

            CreateMap<LectureToUpdate, Domain.Lecture>()
                .ReverseMap();

            CreateMap<HomeworkToUpdate, Domain.Homework>()
                .ReverseMap();

            CreateMap<Domain.Journal, JournalOutput>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Student.Username))
                .ForMember(dest => dest.LectureName, opt => opt.MapFrom(src => src.Lecture.LectureName))
                .ForMember(dest => dest.LectureDate, opt => opt.MapFrom(src => src.Lecture.LectureDate))
                .ReverseMap();

        }
    }
}
