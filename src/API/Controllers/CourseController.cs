
using Microsoft.AspNetCore.Mvc;
using Multilevelteam.Platform.Application.Dtos.CourseDtos;
using Multilevelteam.Platform.Application.Dtos.CourseUpdateDtos;
using Multilevelteam.Platform.Application.Interfaces;
using System.Security.Claims;


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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(Guid id, [FromBody] CourseUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest("Id mos emas!");

            var updateCourse = await _service.UpdateAsync(dto);
            if (updateCourse == null)
            {
                return NotFound();
            }

            return Ok(updateCourse);
        }

        [HttpPut("{id}/teacher")]
        public async Task<IActionResult> UpdateTeacher(Guid id, [FromBody] CourseTeacherIdDto dto)
        {
            var result = await _service.UpdateTeacherAsync(id, dto);

            if (!result)
                return NotFound(new { message = "Kurs topilmadi" });

            return NoContent(); // ✅ Muvaffaqiyatli yangilandi
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result)
                return NotFound(new { message = "Kurs topilmadi" });

            return NoContent(); // ✅ 204 – muvaffaqiyatli o‘chirildi
        }
        private Guid GetUserId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            return Guid.Parse(claim);
        }


        // Kurs o'qituvchisini yangilash
    }
}