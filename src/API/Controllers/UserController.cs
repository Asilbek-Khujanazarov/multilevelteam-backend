using Autotest.Platform.API.DTOs.Users;
using Autotest.Platform.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Autotest.Platform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("me")]
        public async Task<ActionResult<UserMeDto>> GetCurrentUser()
        {
            var userId = User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var id))
                return Unauthorized();

            var user = await _userService.GetCurrentUserAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("phone/{phoneNumber}")]
        public async Task<ActionResult<UserDto>> GetUserByPhoneNumber(string phoneNumber)
        {
            var user = await _userService.GetUserByPhoneNumberAsync(phoneNumber);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("telegram/{chatId}")]
        public async Task<ActionResult<UserDto>> GetUserByTelegramChatId(string chatId)
        {
            var user = await _userService.GetUserByTelegramChatIdAsync(chatId);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("telegram-info/{phoneNumber}")]
        public async Task<ActionResult<TelegramUserDto>> GetTelegramUserByPhone(string phoneNumber)
        {
            var telegramUser = await _userService.GetTelegramUserByPhoneNumberAsync(phoneNumber);
            if (telegramUser == null)
                return NotFound();

            return Ok(telegramUser);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}