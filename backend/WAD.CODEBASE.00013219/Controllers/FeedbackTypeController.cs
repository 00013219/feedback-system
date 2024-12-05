using Microsoft.AspNetCore.Mvc;
using WAD.CODEBASE._00013219.Enums;

namespace WAD.CODEBASE._00013219.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackTypesController : ControllerBase
    {
        // GET: api/FeedbackTypes
        [HttpGet]
        public IActionResult GetFeedbackTypes()
        {
            var feedbackTypes = Enum.GetValues(typeof(FeedbackType))
                .Cast<FeedbackType>()
                .Select(f => new 
                {
                    Id = (int)f,
                    Name = f.ToString()
                }).ToList();

            return Ok(feedbackTypes);
        }

        // GET: api/FeedbackTypes/1
        [HttpGet("{id}")]
        public IActionResult GetFeedbackTypeById(int id)
        {
            var feedbackType = Enum.GetValues(typeof(FeedbackType))
                .Cast<FeedbackType>()
                .FirstOrDefault(f => (int)f == id);
            
            if (!Enum.IsDefined(typeof(FeedbackType), id))
            {
                return NotFound(new { Message = "Feedback type not found" });
            }

            return Ok(new 
            {
                Id = id,
                Name = feedbackType.ToString()
            });
        }
    }
}