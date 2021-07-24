using System;
using System.Collections.Generic;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using module_10.DL.Interfaces;
using module_10.DL.Models;
using module_10.Models;

namespace module_10.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService<User> _studentService;
        private readonly IMapper _mapper;

        public StudentController(IStudentService<User> studentService, IMapper mapper)
        {
            _studentService = studentService;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetStudentById")]
        [ProducesResponseType(typeof(UserOutput), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult GetStudent(string id)
        {
            var user = _studentService.Get(Guid.Parse(id));
            var resultUser = _mapper.Map<UserOutput>(user);
            return Ok(resultUser);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<User>), (int)HttpStatusCode.OK)]
        public IActionResult GetStudents()
        {
            var resultUsers = _mapper.Map<IEnumerable<UserOutput>>(_studentService.GetAllStudents());
            return Ok(resultUsers);
        }

        [HttpPost]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult CreateStudent(UserInput user)
        {
            var newUser = _studentService.Create(_mapper.Map<User>(user));
            return CreatedAtRoute("GetStudentById", new { id = newUser.Id }, newUser);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult UpdateStudent(string id, UserInput user)
        {
            var newUser = _studentService.Update(Guid.Parse(id), _mapper.Map<User>(user));
            return Ok(newUser);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult Delete(string id)
        {
            var deleteUser = _studentService.Delete(Guid.Parse(id));
            return Ok();
        }
    }
}
