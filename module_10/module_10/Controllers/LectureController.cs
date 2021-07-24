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
    public class LectureController : ControllerBase
    {
        private readonly ILectureService<Lecture> _lectureService;
        private readonly IMapper _mapper;

        public LectureController(ILectureService<Lecture> lectureService, IMapper mapper)
        {
            _lectureService = lectureService;
            _mapper = mapper;
        }
        [HttpGet("{id}", Name = "GetLectureById")]
        [ProducesResponseType(typeof(LectureOutput), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult GetLecture(string id)
        {
            var lecture = _lectureService.Get(Guid.Parse(id));
            var resultLecture = _mapper.Map<LectureOutput>(lecture);
            return Ok(resultLecture);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LectureOutput>), (int)HttpStatusCode.OK)]
        public IActionResult GetLectures()
        {
            var lectures = _lectureService.GetAllLectures();
            var resultLectures = _mapper.Map<IEnumerable<LectureOutput>>(lectures);
            return Ok(resultLectures);
        }

        [HttpPost]
        [ProducesResponseType(typeof(LectureOutput), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult CreateLecture(LectureInput lecture)
        {
            var newLecture = _lectureService.Create(_mapper.Map<Lecture>(lecture));
            var resultLecture = _mapper.Map<LectureOutput>(newLecture);
            return CreatedAtRoute("GetLectureById", new {id = resultLecture.Id}, resultLecture);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(LectureOutput), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult UpdateLecture(string id, LectureToUpdate lecture)
        {
            var newLecture = _lectureService.Update(Guid.Parse(id), _mapper.Map<Lecture>(lecture));
            var resultLecture = _mapper.Map<LectureOutput>(newLecture);
            return Ok(resultLecture);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult Delete(string id)
        {
            var deleteLecture = _lectureService.Delete(Guid.Parse(id));
            return Ok();
        }
    }
}
