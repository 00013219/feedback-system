using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using WAD.CODEBASE._00013219.DTOs;
using WAD.CODEBASE._00013219.Repositories;
using WAD.CODEBASE._00013219.Services;

namespace WAD.CODEBASE._00013219.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;

        public LoginController(IUserRepository userRepository, JwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            var user = await _userRepository.GetByEmail(loginDto.Email);
            if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                return Unauthorized(new { Message = "Invalid email or password." });
            }

            var token = _jwtService.GenerateToken(user.UserID.ToString(), user.Role);
            return Ok(new { Token = token });
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hashedPassword = Convert.ToBase64String(hashedBytes);
            return hashedPassword == storedHash;
        }
    }
}