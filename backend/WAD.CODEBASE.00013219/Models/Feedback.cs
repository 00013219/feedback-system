using WAD.CODEBASE._00013219.Enums;

namespace WAD.CODEBASE._00013219.Models
{
    public class Feedback
    {
        public int FeedbackID { get; set; }
        public int UserID { get; set; }
        public FeedbackType FeedbackType { get; set; }
        public string FeedbackContent { get; set; }
        public FeedbackStatus Status { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}