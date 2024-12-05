using Microsoft.EntityFrameworkCore;
using WAD.CODEBASE._00013219.Models;

namespace WAD.CODEBASE._00013219.Repositories
{
    public class UserRepository : IGenericRepository<User>, IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.Include(u => u.Feedbacks).Include(u => u.Comments).ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await _context.Users
                .Include(u => u.Feedbacks)
                .Include(u => u.Comments)
                .FirstOrDefaultAsync(u => u.UserID == id);
        }

        public async Task<bool> ExistsByEmail(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> Create(User entity)
        {
            _context.Users.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<User> Update(User entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<bool> Exists(int userId)
        {
            return await _context.Users.AnyAsync(u => u.UserID == userId);
        }
        
        public async Task<bool> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
        
    }
}