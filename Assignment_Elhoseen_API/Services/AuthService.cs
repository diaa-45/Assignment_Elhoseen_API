using Assignment_Elhoseen_API.Data;
using Assignment_Elhoseen_API.DTOs;
using Assignment_Elhoseen_API.Handler;
using Assignment_Elhoseen_API.Interfaces;
using Assignment_Elhoseen_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EcommerceAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly TokenService _tokenService;
        private readonly JwtSettings _jwtSettings;

        // لحفظ الـ RefreshTokens
        private static readonly Dictionary<string, string> _refreshTokens = new();

        public AuthService(ApplicationDbContext context, TokenService tokenService, IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _tokenService = tokenService;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<ResponseDto?> RegisterAsync(UserRegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.UserName == dto.UserName || u.Email == dto.Email))
                return null;

            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                LastLoginTime = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return await GenerateTokensAsync(user);
        }

        public async Task<ResponseDto?> LoginAsync(UserLoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == dto.UserName);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                return null;

            user.LastLoginTime = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return await GenerateTokensAsync(user);
        }

        public async Task<ResponseDto?> RefreshTokenAsync(string refreshToken)
        {
            if (!_refreshTokens.ContainsKey(refreshToken))
                return null;

            var username = _refreshTokens[refreshToken];
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
                return null;

            return await GenerateTokensAsync(user);
        }

        private async Task<ResponseDto> GenerateTokensAsync(User user)
        {
            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            // احفظ الـ refreshToken مع الـ user
            _refreshTokens[refreshToken] = user.UserName;

            return new ResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes)
            };
        }
    }
}
