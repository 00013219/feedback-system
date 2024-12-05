using WAD.CODEBASE._00013219.Enums;

namespace WAD.CODEBASE._00013219.DTOs
{
    public class FeedbackResponseDto
    {
        public int FeedbackID { get; set; }
        public FeedbackType FeedbackType { get; set; } 
        public string FeedbackContent { get; set; }
        public FeedbackStatus Status { get; set; } 
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; }
        public int UserID { get; set; }
    }
}