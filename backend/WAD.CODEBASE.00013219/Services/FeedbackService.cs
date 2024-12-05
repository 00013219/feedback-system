using WAD.CODEBASE._00013219.DTOs;
using WAD.CODEBASE._00013219.Models;
using WAD.CODEBASE._00013219.Repositories;

namespace WAD.CODEBASE._00013219.Services
{
    public class FeedbackService
    {
        private readonly IGenericRepository<Feedback> _feedbackRepository;
        private readonly CommentRepository _commentRepository;
        private readonly UserRepository _userRepository;

        public FeedbackService(IGenericRepository<Feedback> feedbackRepository, CommentRepository commentRepository, UserRepository userRepository)
        {
            _feedbackRepository = feedbackRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<FeedbackResponseDto>> GetAllFeedbacks()
        {
            var feedbacks = await _feedbackRepository.GetAll();
            return feedbacks.Select(f => new FeedbackResponseDto
            {
                FeedbackID = f.FeedbackID,
                FeedbackType = f.FeedbackType,
                FeedbackContent = f.FeedbackContent,
                Status = f.Status,
                CreatedAt = f.CreatedAt,
                UserName = f.User.Name,
                UserID = f.User.UserID
            });
        }


        public async Task<FeedbackResponseDto> GetFeedbackById(int id)
        {
            var feedback = await _feedbackRepository.GetById(id);
            if (feedback == null) return null;

            return new FeedbackResponseDto
            {
                FeedbackID = feedback.FeedbackID,
                FeedbackType = feedback.FeedbackType,
                FeedbackContent = feedback.FeedbackContent,
                Status = feedback.Status,
                CreatedAt = feedback.CreatedAt,
                UserName = feedback.User.Name,
                UserID = feedback.User.UserID
            };
        }


        public async Task<Feedback> CreateFeedback(int userId, FeedbackRequestDto feedbackDto)
        {
            var user = await _userRepository.GetById(userId);
            if (user == null)
            {
                throw new ArgumentException("User does not exist.");
            }

            var feedback = new Feedback
            {
                User = user,  
                FeedbackType = feedbackDto.FeedbackType,
                FeedbackContent = feedbackDto.FeedbackContent,
                Status = feedbackDto.Status,
                CreatedAt = DateTime.UtcNow
            };
            return await _feedbackRepository.Create(feedback);
        }


        public async Task<Feedback> UpdateFeedback(int id, FeedbackRequestDto feedbackDto)
        {
            var existingFeedback = await _feedbackRepository.GetById(id);

            if (existingFeedback == null) 
                return null;

            existingFeedback.FeedbackType = feedbackDto.FeedbackType;
            existingFeedback.FeedbackContent = feedbackDto.FeedbackContent;
            existingFeedback.Status = feedbackDto.Status;

            return await _feedbackRepository.Update(existingFeedback);
        }

        public async Task<bool> DeleteFeedback(int id)
        {
            var existingFeedback = await _feedbackRepository.GetById(id);

            if (existingFeedback == null) 
                return false;

            return await _feedbackRepository.Delete(id);
        }
        public async Task<IEnumerable<Comment>> GetCommentsForFeedback(int feedbackId)
        {
            return await _commentRepository.GetCommentsByFeedbackId(feedbackId);
        }
    }
}
