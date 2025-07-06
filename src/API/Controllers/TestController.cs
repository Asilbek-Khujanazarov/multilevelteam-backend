using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Multilevelteam.Platform.Domain.DTOs.Exam;
using Multilevelteam.Platform.Domain.Interfaces;

namespace Multilevelteam.Platform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly ITestSessionService _testSessionService;

    public TestController(ITestSessionService testSessionService)
    {
        _testSessionService = testSessionService;
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartTest()
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var result = await _testSessionService.StartTestAsync(userId.Value);
        return Ok(result);
    }

    [HttpPost("{sessionId}/answer")]
    public async Task<IActionResult> Answer(Guid sessionId, [FromBody] AnswerInputDto dto)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        await _testSessionService.SubmitAnswerAsync(userId.Value, sessionId, dto.QuestionId, dto.AnswerId);
        return Ok();
    }

    [HttpPost("{sessionId}/finish")]
    public async Task<IActionResult> Finish(Guid sessionId)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var result = await _testSessionService.FinishTestAsync(userId.Value, sessionId);
        return Ok(result);
    }

    private Guid? GetUserId()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        return Guid.TryParse(userIdStr, out var userId) ? userId : null;
    }
}