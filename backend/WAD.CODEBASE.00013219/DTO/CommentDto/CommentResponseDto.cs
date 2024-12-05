namespace WAD.CODEBASE._00013219.DTOs
{
    public class CommentResponseDto
    {
        public int CommentID { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int FeedbackID { get; set; }
        public int UserID { get; set; }
        
        public string UserName { get; set; }    
    }
}