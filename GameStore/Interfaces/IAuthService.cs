using GameStore.Dtos.LoginDtos;
using GameStore.Dtos.RegisterDtos;
using GameStore.Dtos.TokenDtos;
using GameStore.Entities;

namespace GameStore.Interfaces
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(RegisterUserDto request,string Role);
        Task<TokenResponseDto?> LoginAsync(LoginUserDto request);
        Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request);

    }
}
