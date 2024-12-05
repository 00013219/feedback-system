using System.ComponentModel.DataAnnotations.Schema;

namespace WAD.CODEBASE._00013219.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public string PasswordHash { get; set; }
        
        [NotMapped]
        public string Password { get; set; }

        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
         
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}