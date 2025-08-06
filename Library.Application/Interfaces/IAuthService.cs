using Library.Application.DTOs.Auth;


namespace Library.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterRequest request);
        Task<string> LoginAsync(LoginRequest request);
        Task<string> GenerateResetPasswordTokenAsync(string email);
        Task ResetPasswordAsync(string email, string token, string newPassword);

    }
}
