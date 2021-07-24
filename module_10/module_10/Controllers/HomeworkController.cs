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
    public class HomeworkController : ControllerBase
    {
        private readonly IHomeworkService<Homework> _homeworkService;
        private readonly IMapper _mapper;

        public HomeworkController(IHomeworkService<Homework> homeworkService, IMapper mapper)
        {
            _homeworkService = homeworkService;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetHomeworkById")]
        [ProducesResponseType(typeof(HomeworkOutput), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult GetHomework(string id)
        {
            var homework = _homeworkService.Get(Guid.Parse(id));
            var resultHomework = _mapper.Map<HomeworkOutput>(homework);
            return Ok(resultHomework);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<HomeworkOutput>), (int)HttpStatusCode.OK)]
        public IActionResult GetHomeworks()
        {
            var homeworks = _homeworkService.GetAllHomeworks();
            var resultHomeworks = _mapper.Map<IEnumerable<HomeworkOutput>>(homeworks);
            return Ok(resultHomeworks);
        }

        [HttpPost]
        [ProducesResponseType(typeof(HomeworkOutput), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult CreateHomework(HomeworkToLectureInput homeworkToLecture)
        {
            var newHomework = _homeworkService.Create(_mapper.Map<Homework>(homeworkToLecture));
            var resultHomework = _mapper.Map<HomeworkOutput>(newHomework);
            return CreatedAtRoute("GetHomeworkById", new { id = resultHomework.Id }, resultHomework);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(HomeworkOutput), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult UpdateHomework(string id, HomeworkToUpdate homework)
        {
            var newHomework = _homeworkService.Update(Guid.Parse(id), _mapper.Map<Homework>(homework));
            var resultHomework = _mapper.Map<HomeworkOutput>(newHomework);
            return Ok(resultHomework);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(HomeworkOutput), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult Delete(string id)
        {
            var deleteHomework = _homeworkService.Delete(Guid.Parse(id));
            return Ok();
        }

    }
}
