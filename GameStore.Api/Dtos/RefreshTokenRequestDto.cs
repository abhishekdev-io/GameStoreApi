using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record RefreshTokenRequestDto(
    [property: Required] Guid UserId,
    [property: Required, MinLength(20, ErrorMessage = "Refresh token is too short")] string RefreshToken
);
