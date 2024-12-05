using WAD.CODEBASE._00013219.DTOs;
using WAD.CODEBASE._00013219.Models;
using WAD.CODEBASE._00013219.Repositories;

namespace WAD.CODEBASE._00013219.Services
{
    public class UserService
    {
        private readonly IGenericRepository<User> _userRepository;

        public UserService(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsers()
        {
            var users = await _userRepository.GetAll();
            return users.Select(user => new UserResponseDto
            {
                UserID = user.UserID,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            });
        }

        public async Task<UserResponseDto> GetUserById(int id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null) return null;

            return new UserResponseDto
            {
                UserID = user.UserID,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }

        public async Task<User> CreateUser(UserRequestDto userRequest)
        {
            if (string.IsNullOrEmpty(userRequest.Password) || userRequest.Password.Length < 6)
            {
                throw new ArgumentException("Password must be at least 6 characters long.");
            }
            
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRequest.Password);

            var user = new User
            {
                Name = userRequest.Name,
                Email = userRequest.Email,
                Role = userRequest.Role,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PasswordHash = hashedPassword 
            };
            
            return await _userRepository.Create(user);
        }

        public async Task<UserResponseDto> UpdateUser(int id, UserRequestDto userRequest)
        {
            var user = await _userRepository.GetById(id);
            if (user == null) return null;
            
            user.Name = userRequest.Name;
            user.Email = userRequest.Email;
            user.Role = userRequest.Role;
            user.UpdatedAt = DateTime.UtcNow;

            var updatedUser = await _userRepository.Update(user);
            
            return new UserResponseDto
            {
                UserID = updatedUser.UserID,
                Name = updatedUser.Name,
                Email = updatedUser.Email,
                Role = updatedUser.Role,
                CreatedAt = updatedUser.CreatedAt,
                UpdatedAt = updatedUser.UpdatedAt
            };
        }

        public async Task<bool> DeleteUser(int id)
        {
            return await _userRepository.Delete(id);
        }
    }
}