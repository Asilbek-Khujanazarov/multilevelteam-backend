using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multilevelteam.Platform.Application.Dtos;
using Multilevelteam.Platform.Application.Dtos.CourseDtos;
using Multilevelteam.Platform.Application.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Multilevelteam.Platform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _service;
        public CourseController(ICourseService service)
        {
            _service = service;
        }

        // Kurs qo'shish (faqat teacher yoki admin)
        [HttpPost]
        // [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> Create([FromBody] CreateCourseDto dto)
        {
            var teacherId = GetUserId();
            var result = await _service.CreateAsync(dto, teacherId);
            return Ok(result);
        }

        // Barcha kurslar ro'yxati
        [HttpGet]
        // [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        // Bitta kursni olish
        [HttpGet("{id}")]
        // [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        private Guid GetUserId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            return Guid.Parse(claim);
        }

       // Kurs o'qituvchisini yangilash
    }
}