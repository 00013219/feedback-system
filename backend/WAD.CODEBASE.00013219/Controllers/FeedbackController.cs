using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WAD.CODEBASE._00013219.DTOs;
using WAD.CODEBASE._00013219.Models;
using WAD.CODEBASE._00013219.Services;

namespace WAD.CODEBASE._00013219.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FeedbacksController : ControllerBase
    {
        private readonly FeedbackService _feedbackService;

        public FeedbacksController(FeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        // GET: api/Feedbacks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedbackResponseDto>>> GetFeedbacks()
        {
            var feedbacks = await _feedbackService.GetAllFeedbacks();
            return Ok(feedbacks);
        }

        // GET: api/Feedbacks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Feedback>> GetFeedback(int id)
        {
            var feedback = await _feedbackService.GetFeedbackById(id);

            if (feedback == null)
            {
                return NotFound();
            }

            return Ok(feedback);
        }

        // POST: api/Feedbacks
        [HttpPost]
        public async Task<ActionResult> PostFeedback([FromBody] FeedbackRequestDto feedbackDto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized(new { message = "Invalid or missing token." });
                }
                
                if (!int.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized(new { message = "Invalid user ID in token." });
                }
                
                var createdFeedback = await _feedbackService.CreateFeedback(userId, feedbackDto);

                return CreatedAtAction(nameof(GetFeedback), new { id = createdFeedback.FeedbackID }, new
                {
                    Message = "Feedback created successfully",
                    Feedback = createdFeedback
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        // PUT: api/Feedbacks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeedback(int id, [FromBody] FeedbackRequestDto feedbackDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Invalid user token." });
                }
                
                var feedback = await _feedbackService.GetFeedbackById(id);
                if (feedback == null)
                {
                    return NotFound(new { message = "Feedback not found." });
                }

                if (feedback.UserID.ToString() != userId)
                {
                    return Forbid();
                }
                
                var updatedFeedback = await _feedbackService.UpdateFeedback(id, feedbackDto);

                return Ok(new
                {
                    Message = "Feedback updated successfully",
                    Feedback = updatedFeedback
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/Feedbacks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "Invalid user token." });
            }
            
            var feedback = await _feedbackService.GetFeedbackById(id);
            if (feedback == null)
            {
                return NotFound(new { message = "Feedback not found." });
            }

            if (feedback.UserID.ToString() != userId)
            {
                return Forbid();
            }
            
            var deleted = await _feedbackService.DeleteFeedback(id);

            if (!deleted)
            {
                return BadRequest(new { message = "Failed to delete feedback." });
            }

            return Ok(new { Message = "Feedback deleted successfully" });
        }

        // GET: api/Feedbacks/5/comments
        [HttpGet("{feedbackId}/comments")]
        public async Task<ActionResult<IEnumerable<CommentResponseDto>>> GetCommentsByFeedback(int feedbackId)
        {
            var comments = await _feedbackService.GetCommentsForFeedback(feedbackId);

            if (comments == null || !comments.Any())
            {
                return NotFound();
            }

            var commentDtos = comments.Select(c => new CommentResponseDto
            {
                CommentID = c.CommentID,
                CommentText = c.CommentText,
                UserName = c.User.Name,
                CreatedAt = c.CreatedAt
            });

            return Ok(commentDtos);
        }
    }
}
