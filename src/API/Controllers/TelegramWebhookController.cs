// src/API/Controllers/TelegramWebhookController.cs
using Multilevelteam.Platform.Domain.Interfaces;
using Multilevelteam.Platform.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace Multilevelteam.Platform.API.Controllers
{
    [ApiController]
    [Route("api/telegram")]
    public class TelegramWebhookController : ControllerBase
    {
        private readonly ITelegramBotService _botService;
        private readonly ILogger<TelegramWebhookController> _logger;

        public TelegramWebhookController(
            ITelegramBotService botService,
            ILogger<TelegramWebhookController> logger)
        {
            _botService = botService;
            _logger = logger;
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            try
            {
                _logger.LogInformation("Received update: {@Update}", update);

                if (update == null)
                {
                    _logger.LogWarning("Received null update");
                    return BadRequest("Update object is null");
                }

                await _botService.HandleUpdateAsync(update);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling telegram webhook");
                return BadRequest(new { error = ex.Message });
            }
        }

        // Debug endpoint
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { status = "Webhook controller is working!" });
        }
    }
}