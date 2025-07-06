using Multilevelteam.Platform.API.DTOs.Users;
using Multilevelteam.Platform.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Multilevelteam.Platform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly CloudinaryService _cloudinaryService;
        public UserController(IUserService userService, CloudinaryService cloudinaryService)
        {
            _userService = userService;
            _cloudinaryService = cloudinaryService;
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

        [HttpPost("avatar")]
        public async Task<IActionResult> UploadAvatar([FromForm] AvatarUploadDto dto)
        {
            var file = dto.File;
            if (file == null || file.Length == 0)
                return BadRequest("Fayl tanlanmagan");

            var userId = User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var id))
                return Unauthorized();

            var user = await _userService.GetDomainUserByIdAsync(id); // <-- domain User
            if (user == null)
                return NotFound();

            // Eski avatarni o'chirish
            if (!string.IsNullOrEmpty(user.AvatarPublicId))
                await _cloudinaryService.DeleteImageAsync(user.AvatarPublicId);

            // Yangi avatarni yuklash
            var uploadResult = await _cloudinaryService.UploadImageAsync(file);

            user.AvatarUrl = uploadResult.Url;
            user.AvatarPublicId = uploadResult.PublicId;
            await _userService.UpdateUserAsync(user);

            return Ok(new { avatarUrl = user.AvatarUrl });
        }
    }
}