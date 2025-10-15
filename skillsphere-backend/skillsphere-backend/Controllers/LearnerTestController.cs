using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using skillsphere.core.Dtos;
using skillsphere.core.Interfaces.Services;

namespace skillsphere_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LearnerTestController : ControllerBase
    {
        private readonly ITestService _testService;

        public LearnerTestController(ITestService testService)
        {
            _testService = testService;
        }

        [HttpPost("submit-test")]
        public async Task<IActionResult> SubmitTest([FromBody] SubmitTestRequest request)
        {
            await _testService.SubmitTestAsync(request);
            return Ok(new { message = "Test submitted successfully" });
        }

        [HttpGet("results/{userId}")]
        public async Task<IActionResult> GetResults(int userId)
        {
            var results = await _testService.GetUserResultsAsync(userId);
            return Ok(results);
        }
    }
}
