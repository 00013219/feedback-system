using WAD.CODEBASE._00013219.DTOs;
using WAD.CODEBASE._00013219.Models;
using WAD.CODEBASE._00013219.Repositories;

namespace WAD.CODEBASE._00013219.Services;

public class CommentService
{
    private readonly IGenericRepository<Comment> _commentRepository;
    private readonly IGenericRepository<User> _userRepository;  
    private readonly IGenericRepository<Feedback> _feedbackRepository; 

    public CommentService(
        IGenericRepository<Comment> commentRepository,
        IGenericRepository<User> userRepository,
        IGenericRepository<Feedback> feedbackRepository)
    {
        _commentRepository = commentRepository;
        _userRepository = userRepository;
        _feedbackRepository = feedbackRepository;
    }

    public async Task<IEnumerable<CommentResponseDto>> GetAllComments()
    {
        var comments = await _commentRepository.GetAll();
        return comments.Select(c => new CommentResponseDto
        {
            CommentID = c.CommentID,
            CommentText = c.CommentText,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt,
            FeedbackID = c.FeedbackID,
            UserID = c.UserID
        });
    }

    public async Task<CommentResponseDto> GetCommentById(int id)
    {
        var comment = await _commentRepository.GetById(id);
        if (comment == null) return null;

        return new CommentResponseDto
        {
            CommentID = comment.CommentID,
            CommentText = comment.CommentText,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt,
            FeedbackID = comment.FeedbackID,
            UserID = comment.UserID
        };
    }

    public async Task<CommentResponseDto> CreateComment(CommentRequestDto createCommentDto)
    {
        if (!await _feedbackRepository.Exists(createCommentDto.FeedbackID))
        {
            throw new ArgumentException("Feedback not found.");
        }

        if (!await _userRepository.Exists(createCommentDto.UserID))
        {
            throw new ArgumentException("User not found.");
        }

        var comment = new Comment
        {
            CommentText = createCommentDto.CommentText,
            FeedbackID = createCommentDto.FeedbackID,
            UserID = createCommentDto.UserID
        };

        var createdComment = await _commentRepository.Create(comment);

        return new CommentResponseDto
        {
            CommentID = createdComment.CommentID,
            CommentText = createdComment.CommentText,
            CreatedAt = createdComment.CreatedAt,
            UpdatedAt = createdComment.UpdatedAt,
            FeedbackID = createdComment.FeedbackID,
            UserID = createdComment.UserID
        };
    }

    public async Task<CommentResponseDto> UpdateComment(int id, CommentUpdateDto updateCommentDto)
    {
        var comment = await _commentRepository.GetById(id);
        if (comment == null) return null;

        comment.CommentText = updateCommentDto.CommentText;
        comment.UpdatedAt = DateTime.UtcNow;

        var updatedComment = await _commentRepository.Update(comment);

        return new CommentResponseDto
        {
            CommentID = updatedComment.CommentID,
            CommentText = updatedComment.CommentText,
            CreatedAt = updatedComment.CreatedAt,
            UpdatedAt = updatedComment.UpdatedAt,
            FeedbackID = updatedComment.FeedbackID,
            UserID = updatedComment.UserID,
            UserName = updatedComment.User.Name
        };
    }

    public async Task<bool> DeleteComment(int id)
    {
        return await _commentRepository.Delete(id);
    }
}