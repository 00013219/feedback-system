using System.ComponentModel.DataAnnotations;
using WAD.CODEBASE._00013219.Enums;
using WAD.CODEBASE._00013219.Models;

namespace WAD.CODEBASE._00013219.DTOs
{
    public class FeedbackRequestDto
    {
        [Required(ErrorMessage = "FeedbackType is required")]
        [EnumDataType(typeof(FeedbackType), ErrorMessage = "Invalid FeedbackType")]
        public FeedbackType FeedbackType { get; set; }

        [Required(ErrorMessage = "FeedbackContent is required")]
        [StringLength(1000, ErrorMessage = "Feedback content must be less than 1000 characters.")]
        public string FeedbackContent { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [EnumDataType(typeof(FeedbackStatus), ErrorMessage = "Invalid FeedbackStatus")]
        public FeedbackStatus Status { get; set; }
    }
}