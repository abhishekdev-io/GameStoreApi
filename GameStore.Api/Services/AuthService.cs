using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GameStore.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly GameStoreContext context;
        private readonly JwtSettings jwt;
        private readonly IPasswordHasher<User> passwordHasher;

        public AuthService(GameStoreContext context, IOptions<JwtSettings> jwtOptions, IPasswordHasher<User> passwordHasher) 
        {
            this.context = context;
            jwt = jwtOptions.Value;
            this.passwordHasher = passwordHasher;
        }

        private async Task<User> GetUserByUsername(string username)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (user is null)
                throw new KeyNotFoundException($"User with Username {username} was not found.");
            return user;
        }

        private string CreateToken(User user)
        {

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub,user.UserId.ToString()),
                new(ClaimTypes.Role,user.Role),
                //generate a global unique id for each token
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                //permission-based claims
                new("Permission","Create"),
                new("Permission","Update")
            };

            //admin only claims
            if(user.Role == "Admin")
            {
                claims.Add(new("Permission", "Delete"));
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt.SecretKey));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer:jwt.Issuer,
                audience:jwt.Audience,
                claims:claims,
                expires:DateTime.UtcNow.AddMinutes(jwt.AccessTokenExpirationMinutes),
                signingCredentials: cred
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(jwt.RefreshTokenExpirationDays);
            await context.SaveChangesAsync();
            return refreshToken;

        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }


        private async Task<TokenResponseDto> CreateTokenResponseAsync(User user)
        {
            return new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }

        public async Task<TokenResponseDto?> LoginAsync(LoginUserDto request)
        {
            var user = await GetUserByUsername(request.Username);

            if(passwordHasher.VerifyHashedPassword(user,user.PasswordHash,request.Password) == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Invalid credentials");


            return await CreateTokenResponseAsync(user);
        }

        public async Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            if(user is null)
                return null ;

            return await CreateTokenResponseAsync(user);
        }

        public async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {
            var user = await context.Users.FindAsync(userId);

            if (user is null)
                throw new UnauthorizedAccessException("User not found.");

            if (user.RefreshToken != refreshToken)
                throw new UnauthorizedAccessException("Invalid refresh token.");

            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                throw new UnauthorizedAccessException("Refresh token has expired.");

            return user;
        }

        public async Task<User?> RegisterAsync(RegisterUserDto request,string Role)
        { 

            if (await context.Users.AnyAsync(u => u.Username == request.Username))
                throw new UnauthorizedAccessException("Username already exists");

            var user = new User();

            var hashedPassword = passwordHasher
                .HashPassword(user, request.Password);

            user.Username = request.Username;
            user.PasswordHash = hashedPassword;
            user.CreatedAt = DateTime.UtcNow;
            user.Role = Role;
            user.Email = request.Email;

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return user;
        }
    }
}
