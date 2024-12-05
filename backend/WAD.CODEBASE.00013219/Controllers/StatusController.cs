using Microsoft.AspNetCore.Mvc;
using WAD.CODEBASE._00013219.Enums;

namespace WAD.CODEBASE._00013219.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        // GET: api/FeedbackTypes
        [HttpGet]
        public IActionResult GetFeedbackStatus()
        {
            var feedbackStatus = Enum.GetValues(typeof(FeedbackStatus))
                .Cast<FeedbackStatus>()
                .Select(f => new 
                {
                    Id = (int)f,
                    Name = f.ToString()
                }).ToList();

            return Ok(feedbackStatus);
        }

        // GET: api/FeedbackTypes/1
        [HttpGet("{id}")]
        public IActionResult GetFeedbackStatusById(int id)
        {
            var feedbackStatus = Enum.GetValues(typeof(FeedbackStatus))
                .Cast<FeedbackStatus>()
                .FirstOrDefault(f => (int)f == id);
            
            if (!Enum.IsDefined(typeof(FeedbackStatus), id))
            {
                return NotFound(new { Message = "Feedback type not found" });
            }

            return Ok(new 
            {
                Id = id,
                Name = feedbackStatus.ToString()
            });
        }
    }
}