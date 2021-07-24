using AutoMapper;
using module_10.DAL.University.Entities;
using module_10.DAL.University.Identity;
using Domain = module_10.DL.Models;
namespace module_10.DAL.University.Utils
{
    internal class DataAccessMapperProfile : Profile
    {
        public DataAccessMapperProfile()
        {
            CreateMap<Lecture, Domain.Lecture>()
                .ReverseMap();

            CreateMap<Homework, Domain.Homework>()
                .ReverseMap();

            CreateMap<User, Domain.User>()
                .ReverseMap();
        }
    }
}
