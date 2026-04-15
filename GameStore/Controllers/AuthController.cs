using GameStore.Dtos.LoginDtos;
using GameStore.Dtos.RegisterDtos;
using GameStore.Dtos.TokenDtos;
using GameStore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        /*------------Register User------------*/
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterUserAsync(RegisterUserDto request)
        {
            var user = await authService.RegisterAsync(request, "User");

            if (user is null)
                return BadRequest("User registration failed");

            return Ok(new
            {
                user.UserId,
                user.Username,
                user.Email,
                user.Role
            });
        }

        /*------------User Login------------*/
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<TokenResponseDto>> LoginUserAsync(LoginUserDto request)
        {
            var response = await authService.LoginAsync(request);

            if (response is null)
                return Unauthorized("Login Failed");

            return Ok(response);
        }

        /*------------Refresh Token------------*/
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<ActionResult<TokenResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            var response =await authService.RefreshTokenAsync(request);

            if (response is null)
                return Unauthorized("Invalid or expired refresh token");

            return Ok(response);
        }


        /*------------Register Admin------------*/
        [HttpPost("register-admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> RegisterAdminAsync(RegisterUserDto request)
        {
            var admin = await authService.RegisterAsync(request, "Admin");

            if (admin is null)
                return BadRequest("Admin registration failed");

            return Ok(new
            {
                admin.UserId,
                admin.Username,
                admin.Email,
                admin.Role
            });
        }
    }
}
