using Microsoft.EntityFrameworkCore;
using WAD.CODEBASE._00013219.Models;

namespace WAD.CODEBASE._00013219.Repositories
{
    public class CommentRepository : IGenericRepository<Comment>
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Comment>> GetAll()
        {
            return await _context.Comments
                .Include(c => c.Feedback)
                .Include(c => c.User)
                .ToListAsync();
        }

        public async Task<Comment> GetById(int id)
        {
            return await _context.Comments
                .Include(c => c.Feedback)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CommentID == id);
        }

        public async Task<Comment> Create(Comment entity)
        {
            _context.Comments.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Comment> Update(Comment entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> Delete(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return false;

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Comment>> GetCommentsByFeedbackId(int feedbackId)
        {
            return await _context.Comments
                .Include(c => c.Feedback) 
                .Include(c => c.User) 
                .Where(c => c.FeedbackID == feedbackId)
                .ToListAsync();
        }
        public async Task<bool> Exists(int commentId)
        {
            return await _context.Comments.AnyAsync(u => u.CommentID == commentId);
        }
    }
}