using Microsoft.AspNetCore.Mvc;

namespace Multilevelteam.Platform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly CourseService _courseService;

        public CourseController(CourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto createCourseDto)
        {
            // Implementation for creating a course
            await _courseService.CreateCourseAsync(createCourseDto); 

            return Ok(new { message = "Course created successfully" });   
        }
        
    }
}