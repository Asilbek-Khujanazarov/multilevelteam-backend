using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multilevelteam.Platform.Application.Dtos;
using Multilevelteam.Platform.Application.Dtos.lessonDtos;
using Multilevelteam.Platform.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace Multilevelteam.Platform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _service;
        public LessonController(ILessonService service)
        {
            _service = service;
        }

        // Dars qo'shish (faqat teacher yoki admin)
        [HttpPost]
        // [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> Create([FromBody] CreateLessonDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        // Kursga tegishli barcha darslar
        [HttpGet("by-course/{courseId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByCourseId(Guid courseId)
        {
            var result = await _service.GetByCourseIdAsync(courseId);
            return Ok(result);
        }
    }
}