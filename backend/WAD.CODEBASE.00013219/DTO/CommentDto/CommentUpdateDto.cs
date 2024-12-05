using System.ComponentModel.DataAnnotations;

namespace WAD.CODEBASE._00013219.DTOs
{
    public class CommentUpdateDto
    {
        [Required(ErrorMessage = "Comment text is required.")]
        [StringLength(1000, ErrorMessage = "Comment text cannot exceed 1000 characters.")]
        public string CommentText { get; set; }
    }
}