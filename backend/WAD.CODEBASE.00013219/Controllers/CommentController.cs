using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; 
using WAD.CODEBASE._00013219.DTOs;
using WAD.CODEBASE._00013219.Services;

namespace WAD.CODEBASE._00013219.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CommentsController : ControllerBase
{
    private readonly CommentService _commentService;

    public CommentsController(CommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommentResponseDto>>> GetComments()
    {
        var comments = await _commentService.GetAllComments();
        return Ok(comments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CommentResponseDto>> GetComment(int id)
    {
        var comment = await _commentService.GetCommentById(id);
        if (comment == null)
        {
            return NotFound(new { message = "Comment not found." });
        }
        return Ok(comment);
    }

    [HttpPost]
    public async Task<ActionResult<CommentResponseDto>> PostComment(CommentRequestDto createCommentDto)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            createCommentDto.UserID = int.Parse(userId ?? "0");

            var createdComment = await _commentService.CreateComment(createCommentDto);
            return CreatedAtAction(nameof(GetComment), new { id = createdComment.CommentID }, createdComment);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutComment(int id, [FromBody] CommentUpdateDto updateCommentDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var existingComment = await _commentService.GetCommentById(id);
        if (existingComment == null)
        {
            return NotFound(new { message = "Comment not found." });
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (existingComment.UserID != int.Parse(userId ?? "0"))
        {
            return Forbid();
        }

        try
        {
            var updatedComment = await _commentService.UpdateComment(id, updateCommentDto);
            return Ok(updatedComment);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var existingComment = await _commentService.GetCommentById(id);
        if (existingComment == null)
        {
            return NotFound(new { message = "Comment not found." });
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (existingComment.UserID != int.Parse(userId ?? "0"))
        {
            return Forbid();
        }

        var deleted = await _commentService.DeleteComment(id);
        if (!deleted)
        {
            return NotFound(new { message = "Comment not found." });
        }

        return NoContent();
    }
}
