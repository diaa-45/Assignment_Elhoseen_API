using Assignment_Elhoseen_API.DTOs;

namespace Assignment_Elhoseen_API.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseDto?> RegisterAsync(UserRegisterDto dto);
        Task<ResponseDto?> LoginAsync(UserLoginDto dto);
        Task<ResponseDto?> RefreshTokenAsync(string refreshToken);
    }
}
