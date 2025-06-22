// src/API/Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Autotest.Platform.API.DTOs.Auth;
using Autotest.Platform.Infrastructure.Services;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Domain.Interfaces;

namespace Autotest.Platform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> StartRegistration([FromBody] RegisterRequest request)
        {
            var (success, message) = await _authService.StartRegistrationAsync(request);
            
            if (!success)
                return BadRequest(new { message });

            return Ok(new { message });
        }

        [HttpPost("register/verify")]
        public async Task<IActionResult> CompleteRegistration([FromBody] VerifyCodeRequest request)
        {
            try
            {
                var (tokens, user) = await _authService.CompleteRegistrationAsync(request);
                return Ok(new { tokens, user });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> StartLogin([FromBody] LoginRequest request)
        {
            var (success, message) = await _authService.StartLoginAsync(request);
            
            if (!success)
                return BadRequest(new { message });

            return Ok(new { message });
        }

        [HttpPost("login/verify")]
        public async Task<ActionResult<(TokenResponse tokens, UserResponse user)>> VerifyLogin([FromBody] LoginVerifyRequest request)
            {
            var result = await _authService.CompleteLoginAsync(request);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var tokens = await _authService.RefreshTokenAsync(
                    request.AccessToken, 
                    request.RefreshToken);
                    
                return Ok(tokens);
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("revoke")]
        public async Task<IActionResult> RevokeToken()
        {
            var phoneNumber = User.FindFirst(ClaimTypes.MobilePhone)?.Value;
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return Unauthorized();
            }

            var success = await _authService.RevokeTokenAsync(phoneNumber);
            if (!success)
            {
                return BadRequest(new { message = "Token bekor qilishda xatolik yuz berdi" });
            }

            return Ok(new { message = "Token muvaffaqiyatli bekor qilindi" });
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                var phoneNumber = User.FindFirst(ClaimTypes.MobilePhone)?.Value;
                if (string.IsNullOrEmpty(phoneNumber))
                {
                    return BadRequest(new { message = "Foydalanuvchi ma'lumotlari topilmadi" });
                }

                var (success, message) = await _authService.ChangePasswordAsync(phoneNumber, request);
                if (!success)
                {
                    return BadRequest(new { message });
                }

                return Ok(new { message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Xatolik yuz berdi", error = ex.Message });
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var (success, message) = await _authService.ForgotPasswordAsync(request);
            if (!success)
            {
                return BadRequest(new { message });
            }
            return Ok(new { message });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var (success, message) = await _authService.ResetPasswordAsync(request);
            if (!success)
            {
                return BadRequest(new { message });
            }
            return Ok(new { message });
        }
    }
}