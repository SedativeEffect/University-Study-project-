using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using module_10.DAL.University.Identity;
using module_10.DL.Interfaces;
using Domain = module_10.DL.Models;
namespace module_10.DAL.University.Repositories
{
    internal class StudentsRepository : IRepository<Domain.User>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public StudentsRepository(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public IEnumerable<Domain.User> GetAll()
        {
            var dbUsers = _userManager.Users
                .Include(x => x.Lectures)
                .Include(x => x.Homeworks)
                .ToList();
            return _mapper.Map<IEnumerable<Domain.User>>(dbUsers);
        }

        public Domain.User Get(Guid id)
        {
            var dbUser = _userManager.Users
                .Include(x => x.Lectures)
                .Include(x => x.Homeworks)
                .FirstOrDefault(x => x.Id == id);
            return dbUser is null ? null : _mapper.Map<Domain.User>(dbUser);
        }

        public Domain.User Create(Domain.User entity)
        {
            var dbUser = _mapper.Map<User>(entity);
            var result = _userManager.CreateAsync(dbUser, "password").GetAwaiter().GetResult();
            if (result is not null && result.Succeeded)
            {
                _userManager.AddClaimAsync(dbUser, new Claim(ClaimTypes.Role, entity.Role)).GetAwaiter().GetResult();
                var dtoUser = _mapper.Map<Domain.User>(dbUser);
                return dtoUser;
            }

            return null;
        }

        public Domain.User Update(Domain.User entity)
        {
            if (_userManager.FindByIdAsync(entity.Id.ToString()).GetAwaiter().GetResult() is User dbUser)
            {
                _mapper.Map(entity, dbUser);
                var result = _userManager.UpdateAsync(dbUser).GetAwaiter().GetResult();
                if (result.Succeeded)
                {
                    var dtoUser = _mapper.Map<Domain.User>(dbUser);
                    return dtoUser;
                }
            }

            return null;
        }

        public bool Delete(Guid id)
        {
            if (_userManager.FindByIdAsync(id.ToString()).GetAwaiter().GetResult() is User dbUser)
            {
                var result = _userManager.DeleteAsync(dbUser).GetAwaiter().GetResult();
                if (result.Succeeded)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
