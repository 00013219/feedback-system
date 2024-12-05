using WAD.CODEBASE._00013219.Models;

namespace WAD.CODEBASE._00013219.Repositories
{
    public interface IUserRepository
    {
        Task<bool> ExistsByEmail(string email);
        Task<User> GetByEmail(string email);
        Task<User> Create(User user);
    }
}