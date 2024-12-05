using Microsoft.EntityFrameworkCore;
using WAD.CODEBASE._00013219.Enums;
using WAD.CODEBASE._00013219.Models;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.User)
                .WithMany(u => u.Feedbacks)
                .HasForeignKey(f => f.UserID);
            modelBuilder.Entity<Feedback>()
                .Property(f => f.Status)
                .HasConversion(
                    v => v.ToString(), 
                    v => (FeedbackStatus)Enum.Parse(typeof(FeedbackStatus), v)
                );

            modelBuilder.Entity<Feedback>()
                .Property(f => f.FeedbackType)
                .HasConversion(
                    v => v.ToString(), 
                    v => (FeedbackType)Enum.Parse(typeof(FeedbackType), v)
                );
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Feedback)
                .WithMany(f => f.Comments)
                .HasForeignKey(c => c.FeedbackID);
            
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserID);
            
        }
    }
