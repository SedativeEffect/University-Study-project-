using AutoMapper;
using module_10.DAL.University.Entities;
using module_10.DAL.University.Identity;
using Domain = module_10.DL.Models;
namespace module_10.BLL.Utils
{
    public class BusinessLogicMapperProfile : Profile
    {
        public BusinessLogicMapperProfile()
        {
            CreateMap<Domain.Lecture, Lecture>()
                .ReverseMap();

            CreateMap<Domain.User, User>()
                .ReverseMap();

            CreateMap<Domain.Journal, Journal>()
                .ReverseMap();
        }
    }
}
