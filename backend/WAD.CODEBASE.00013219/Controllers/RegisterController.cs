using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using WAD.CODEBASE._00013219.DTOs;
using WAD.CODEBASE._00013219.Models;
using WAD.CODEBASE._00013219.Repositories;

namespace WAD.CODEBASE._00013219.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public RegisterController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerDto)
        {
            if (await _userRepository.ExistsByEmail(registerDto.Email))
            {
                return Conflict(new { Message = "Email already registered." });
            }

            var user = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Role = "user",
                PasswordHash = HashPassword(registerDto.Password),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _userRepository.Create(user);
            return Ok(new { Message = "User registered successfully." });
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}