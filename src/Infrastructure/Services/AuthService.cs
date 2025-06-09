// src/Infrastructure/Services/AuthService.cs
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Autotest.Platform.Domain.Entities;
using Autotest.Platform.Domain.Interfaces;
using Autotest.Platform.Domain.Enums;
using Autotest.Platform.API.DTOs.Auth;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Autotest.Platform.Infrastructure.Services
{
    public interface IAuthService
    {
        Task<(bool success, string message)> StartRegistrationAsync(RegisterRequest request);
        Task<(TokenResponse tokens, UserResponse user)> CompleteRegistrationAsync(VerifyCodeRequest request);
        Task<(bool success, string message)> StartLoginAsync(LoginRequest request);
        Task<(TokenResponse tokens, UserResponse user)> CompleteLoginAsync(VerifyCodeRequest request);
        Task<TokenResponse> RefreshTokenAsync(string accessToken, string refreshToken);
        Task<bool> RevokeTokenAsync(string phoneNumber);
    }

    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IVerificationCodeRepository _codeRepository;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IConfiguration _configuration;
        private readonly ITelegramBotService _telegramBotService;

        // Tasdiqlash kodini qayta yuborish uchun minimal vaqt
        private readonly TimeSpan _codeResendWindow = TimeSpan.FromMinutes(1);
        
        // Tasdiqlash kodining amal qilish muddati
        private readonly TimeSpan _codeExpiryWindow = TimeSpan.FromMinutes(5);

        public AuthService(
            IUserRepository userRepository,
            IVerificationCodeRepository codeRepository,
            IJwtService jwtService,
            IPasswordHasher passwordHasher,
            IConfiguration configuration,
            ITelegramBotService telegramBotService)
        {
            _userRepository = userRepository;
            _codeRepository = codeRepository;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
            _telegramBotService = telegramBotService;
        }

        public async Task<(bool success, string message)> StartRegistrationAsync(RegisterRequest request)
        {
            // Telefon raqami mavjudligini tekshirish
            if (await _userRepository.ExistsByPhoneNumberAsync(request.PhoneNumber))
            {
                return (false, "Bu telefon raqam allaqachon ro'yxatdan o'tgan");
            }

            // So'nggi yuborilgan kodni tekshirish
            if (await _codeRepository.IsRecentCodeExistsAsync(request.PhoneNumber, _codeResendWindow))
            {
                return (false, $"Iltimos, {_codeResendWindow.TotalMinutes} daqiqa kutib turing");
            }

            // TelegramUser ni tekshirish
            var telegramUser = await _userRepository.GetTelegramUserByPhoneNumberAsync(request.PhoneNumber);
            if (telegramUser == null)
            {
                return (false, "Iltimos, avval Telegram bot orqali ro'yxatdan o'ting");
            }

            // Yangi tasdiqlash kodini yaratish
            var verificationCode = new VerificationCode
            {
                PhoneNumber = request.PhoneNumber,
                Code = GenerateVerificationCode(),
                ExpiryTime = DateTime.UtcNow.Add(_codeExpiryWindow),
                Purpose = VerificationPurpose.Registration
            };

            // Kodni saqlash
            await _codeRepository.CreateAsync(verificationCode);

            // Telegram orqali kodni yuborish
            var chatId = long.Parse(telegramUser.ChatId);
            var sent = await _telegramBotService.SendVerificationCodeAsync(chatId, verificationCode.Code);

            if (!sent)
            {
                return (false, "Tasdiqlash kodini yuborishda xatolik yuz berdi. Iltimos, qayta urinib ko'ring");
            }

            return (true, "Tasdiqlash kodi Telegram orqali yuborildi");
        }

        public async Task<(TokenResponse tokens, UserResponse user)> CompleteRegistrationAsync(VerifyCodeRequest request)
        {
            // Tasdiqlash kodini tekshirish
            var verificationCode = await _codeRepository.GetLatestCodeAsync(
                request.PhoneNumber, 
                VerificationPurpose.Registration);

            if (verificationCode == null || verificationCode.Code != request.Code)
            {
                throw new ApplicationException("Noto'g'ri tasdiqlash kodi");
            }

            if (verificationCode.ExpiryTime < DateTime.UtcNow)
            {
                throw new ApplicationException("Tasdiqlash kodi muddati tugagan");
            }

            if (verificationCode.IsUsed)
            {
                throw new ApplicationException("Tasdiqlash kodi allaqachon ishlatilgan");
            }

            // Foydalanuvchini yaratish
            var user = new User
            {
                PhoneNumber = request.PhoneNumber,
                FirstName = verificationCode.User.FirstName,
                LastName = verificationCode.User.LastName,
                Gender = verificationCode.User.Gender,
                PasswordHash = verificationCode.User.PasswordHash,
                IsVerified = true
            };

            await _userRepository.CreateAsync(user);

            // Kodni ishlatilgan deb belgilash
            verificationCode.IsUsed = true;
            await _codeRepository.UpdateAsync(verificationCode);

            // Token va refresh token yaratish
            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();
            var refreshTokenExpiryTime = _jwtService.GetRefreshTokenExpiryTime();

            // Refresh tokenni saqlash
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiryTime;
            await _userRepository.UpdateAsync(user);

            return (
                new TokenResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    AccessTokenExpiration = DateTime.UtcNow.AddMinutes(
                        Convert.ToDouble(_configuration["JWT:ExpirationInMinutes"])),
                    RefreshTokenExpiration = refreshTokenExpiryTime
                },
                MapToUserResponse(user)
            );
        }

        public async Task<(bool success, string message)> StartLoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByPhoneNumberAsync(request.PhoneNumber);
            if (user == null)
            {
                return (false, "Foydalanuvchi topilmadi");
            }

            if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                return (false, "Noto'g'ri parol");
            }

            // So'nggi yuborilgan kodni tekshirish
            if (await _codeRepository.IsRecentCodeExistsAsync(request.PhoneNumber, _codeResendWindow))
            {
                return (false, $"Iltimos, {_codeResendWindow.TotalMinutes} daqiqa kutib turing");
            }

            // TelegramUser ni tekshirish
            var telegramUser = await _userRepository.GetTelegramUserByPhoneNumberAsync(request.PhoneNumber);
            if (telegramUser == null)
            {
                return (false, "Iltimos, avval Telegram bot orqali ro'yxatdan o'ting");
            }

            // Yangi tasdiqlash kodini yaratish
            var verificationCode = new VerificationCode
            {
                PhoneNumber = request.PhoneNumber,
                Code = GenerateVerificationCode(),
                ExpiryTime = DateTime.UtcNow.Add(_codeExpiryWindow),
                Purpose = VerificationPurpose.Login,
                UserId = user.Id
            };

            await _codeRepository.CreateAsync(verificationCode);

            // Telegram orqali kodni yuborish
            var chatId = long.Parse(telegramUser.ChatId);
            var sent = await _telegramBotService.SendVerificationCodeAsync(chatId, verificationCode.Code);

            if (!sent)
            {
                return (false, "Tasdiqlash kodini yuborishda xatolik yuz berdi. Iltimos, qayta urinib ko'ring");
            }

            return (true, "Tasdiqlash kodi Telegram orqali yuborildi");
        }

        public async Task<(TokenResponse tokens, UserResponse user)> CompleteLoginAsync(VerifyCodeRequest request)
        {
            var verificationCode = await _codeRepository.GetLatestCodeAsync(
                request.PhoneNumber, 
                VerificationPurpose.Login);

            if (verificationCode == null || verificationCode.Code != request.Code)
            {
                throw new ApplicationException("Noto'g'ri tasdiqlash kodi");
            }

            if (verificationCode.ExpiryTime < DateTime.UtcNow)
            {
                throw new ApplicationException("Tasdiqlash kodi muddati tugagan");
            }

            if (verificationCode.IsUsed)
            {
                throw new ApplicationException("Tasdiqlash kodi allaqachon ishlatilgan");
            }

            var user = await _userRepository.GetByPhoneNumberAsync(request.PhoneNumber);
            if (user == null)
            {
                throw new ApplicationException("Foydalanuvchi topilmadi");
            }

            // Login vaqtini yangilash
            user.LastLoginDate = DateTime.UtcNow;

            // Token va refresh token yaratish
            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();
            var refreshTokenExpiryTime = _jwtService.GetRefreshTokenExpiryTime();

            // Refresh tokenni saqlash
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiryTime;
            await _userRepository.UpdateAsync(user);

            // Kodni ishlatilgan deb belgilash
            verificationCode.IsUsed = true;
            await _codeRepository.UpdateAsync(verificationCode);

            return (
                new TokenResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    AccessTokenExpiration = DateTime.UtcNow.AddMinutes(
                        Convert.ToDouble(_configuration["JWT:ExpirationInMinutes"])),
                    RefreshTokenExpiration = refreshTokenExpiryTime
                },
                MapToUserResponse(user)
            );
        }

        public async Task<TokenResponse> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(accessToken);
            var userId = Guid.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || 
                user.RefreshToken != refreshToken || 
                user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }

            var newAccessToken = _jwtService.GenerateAccessToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();
            var refreshTokenExpiryTime = _jwtService.GetRefreshTokenExpiryTime();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiryTime;
            await _userRepository.UpdateAsync(user);

            return new TokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_configuration["JWT:ExpirationInMinutes"])),
                RefreshTokenExpiration = refreshTokenExpiryTime
            };
        }

        public async Task<bool> RevokeTokenAsync(string phoneNumber)
        {
            var user = await _userRepository.GetByPhoneNumberAsync(phoneNumber);
            if (user == null)
            {
                return false;
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            await _userRepository.UpdateAsync(user);

            return true;
        }

        private string GenerateVerificationCode()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        private UserResponse MapToUserResponse(User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                IsVerified = user.IsVerified,
                RegisteredDate = user.RegisteredDate,
                LastLoginDate = user.LastLoginDate
            };
        }
    }
}