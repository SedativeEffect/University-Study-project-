using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using module_10.DL.Interfaces;
using module_10.DL.Models;

namespace module_10.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudyController : ControllerBase
    {
        private readonly IStudyService<Journal> _studyService;

        public StudyController(IStudyService<Journal> studyService)
        {
            _studyService = studyService;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult AddStudentToLecture(Guid studentId, Guid lectureId, bool hasHomework)
        {
            var result = _studyService.AddStudentToLecture(studentId, lectureId, hasHomework);
            return Ok();
        }


        [HttpGet]
        [Route("getaveragemark/{studentId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult GetAverageMark(string studentId)
        {
            var result = _studyService.GetAverageMark(Guid.Parse(studentId));
            return Ok(result);
        }
    }
}
