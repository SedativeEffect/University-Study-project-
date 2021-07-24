using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using module_10.DL.Interfaces;
using module_10.DL.Models;
using module_10.Models;

namespace module_10.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IStudyService<Journal> _studyService;
        private readonly IMapper _mapper;

        public ReportController(IStudyService<Journal> studyService, IMapper mapper)
        {
            _studyService = studyService;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("attendance/bylecture/{format}/{lectureName}"), FormatFilter]
        public IActionResult GetAttendanceByLecture(string lectureName)
        {
            var result = _studyService.GetAttendanceByLecture(lectureName);
            return Ok(_mapper.Map<IEnumerable<JournalOutput>>(result));
        }

        [HttpGet]
        [Route("attendance/bystudent/{format}/{studentName}"), FormatFilter]
        public IActionResult GetAttendanceByStudent(string studentName)
        {
            var result = _studyService.GetAttendanceByStudent(studentName);
            return Ok(_mapper.Map<IEnumerable<JournalOutput>>(result));
        }
    }
}
