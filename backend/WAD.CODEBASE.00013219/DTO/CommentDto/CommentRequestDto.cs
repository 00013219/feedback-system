using System.ComponentModel.DataAnnotations;

public class CommentRequestDto
{
    [Required]
    public string CommentText { get; set; }

    [Required]
    public int FeedbackID { get; set; }

    [Required]
    public int UserID { get; set; }
}