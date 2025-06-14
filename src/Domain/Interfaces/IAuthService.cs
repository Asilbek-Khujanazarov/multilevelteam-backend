using System.Threading.Tasks;
using Autotest.Platform.API.DTOs.Auth;

namespace Domain.Interfaces
{
    public interface IAuthService
    {
        Task<(TokenResponse tokens, UserResponse user)> CompleteRegistrationAsync(VerifyCodeRequest request);
        Task<(bool success, string message)> StartLoginAsync(LoginRequest request);
        Task<(TokenResponse tokens, UserResponse user)> CompleteLoginAsync(LoginVerifyRequest request);
        Task<TokenResponse> RefreshTokenAsync(string accessToken, string refreshToken);
        Task<bool> RevokeTokenAsync(string phoneNumber);
        Task<(bool success, string message)> ChangePasswordAsync(string phoneNumber, ChangePasswordRequest request);
        Task<(bool success, string message)> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<(bool success, string message)> ResetPasswordAsync(ResetPasswordRequest request);
    }
} 