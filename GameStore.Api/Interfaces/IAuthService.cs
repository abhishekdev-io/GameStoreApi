using GameStore.Api.Dtos;
using GameStore.Api.Entities;

namespace GameStore.Api.Interfaces
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(RegisterUserDto request,string Role);
        Task<TokenResponseDto?> LoginAsync(LoginUserDto request);
        Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request);

    }
}
