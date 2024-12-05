using Microsoft.EntityFrameworkCore;
using WAD.CODEBASE._00013219.Models;

namespace WAD.CODEBASE._00013219.Repositories
{
    public class FeedbackRepository : IGenericRepository<Feedback>
    {
        private readonly AppDbContext _context;

        public FeedbackRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Feedback>> GetAll()
        {
            return await _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Comments)
                .ToListAsync();
        }

        public async Task<Feedback> GetById(int id)
        {
            return await _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Comments)
                .FirstOrDefaultAsync(f => f.FeedbackID == id);
        }

        public async Task<Feedback> Create(Feedback entity)
        {
            _context.Feedbacks.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Feedback> Update(Feedback entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> Delete(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null) return false;

            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> Exists(int feedbackId)
        {
            return await _context.Feedbacks.AnyAsync(u => u.FeedbackID == feedbackId);
        }
    }
}