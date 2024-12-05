namespace WAD.CODEBASE._00013219.Models
{
    public class Comment
    {
        public int CommentID { get; set; }
        public int FeedbackID { get; set; }
        public int UserID { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public Feedback Feedback { get; set; }

        public User User { get; set; }
    }
}
