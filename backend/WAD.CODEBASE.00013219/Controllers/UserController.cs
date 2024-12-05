using Microsoft.AspNetCore.Mvc;
using WAD.CODEBASE._00013219.DTOs;
using WAD.CODEBASE._00013219.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace WAD.CODEBASE._00013219.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUser(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        
        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> PostUser([FromBody] UserRequestDto userRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (string.IsNullOrEmpty(userRequest.Password) || userRequest.Password.Length < 6)
            {
                ModelState.AddModelError("Password", "Password must be at least 6 characters long.");
                return BadRequest(ModelState);
            }
            
            if (!IsValidEmail(userRequest.Email))
            {
                ModelState.AddModelError("Email", "Invalid email format.");
                return BadRequest(ModelState);
            }

            var createdUser = await _userService.CreateUser(userRequest);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.UserID }, createdUser);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, [FromBody] UserRequestDto userRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (!string.IsNullOrEmpty(userRequest.Password) && userRequest.Password.Length < 6)
            {
                ModelState.AddModelError("Password", "Password must be at least 6 characters long.");
                return BadRequest(ModelState);
            }
            
            if (!IsValidEmail(userRequest.Email))
            {
                ModelState.AddModelError("Email", "Invalid email format.");
                return BadRequest(ModelState);
            }

            var updatedUser = await _userService.UpdateUser(id, userRequest);
            if (updatedUser == null)
            {
                return NotFound();
            }

            return Ok(updatedUser);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _userService.DeleteUser(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
        
        private bool IsValidEmail(string email)
        {
            try
            {
                var emailAddr = new System.Net.Mail.MailAddress(email);
                return emailAddr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
