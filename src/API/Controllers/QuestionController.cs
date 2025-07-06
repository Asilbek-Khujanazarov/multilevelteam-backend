using Multilevelteam.Platform.API.DTOs.Questions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class QuestionController : ControllerBase
{
    private readonly IQuestionService _questionService;

    public QuestionController(IQuestionService questionService)
    {
        _questionService = questionService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] QuestionCreateDto dto)
    {
        var question = await _questionService.CreateAsync(dto);
        return Ok( new { message = "Muvaffaqiyatli yaratildi" });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var question = await _questionService.GetByIdAsync(id);
        if (question == null) return NotFound();
        return Ok(question);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var questions = await _questionService.GetAllAsync();
        return Ok(questions);
    }

    [HttpGet("page")]
    public async Task<IActionResult> GetAllAsyncPage([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        var result = await _questionService.GetAllAsyncPage(page, pageSize);
        return Ok(new { questions = result.Questions, total = result.Total });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromForm] QuestionCreateDto dto)
    {
        var result = await _questionService.UpdateAsync(id, dto);
        if (!result) return NotFound();
        return Ok(new { message = "Muvaffaqiyatli yangilandi" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _questionService.DeleteAsync(id);
        if (!result) return NotFound();
        return Ok(new { message = "Muvaffaqiyatli o'chirildi"  });
    }
}