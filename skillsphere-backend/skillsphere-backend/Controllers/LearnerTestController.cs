using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using skillsphere.core.Dtos;
using skillsphere.core.Interfaces.Services;

namespace skillsphere_backend.Controllers
{
    [Route("api/learner/test")]
    [ApiController]
    //[Authorize(Roles = "Learner")]
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


        [HttpGet("{id}")]
        public async Task<IActionResult> GetTestForLearner(int id)
        {
            var test = await _testService.GetTestForLearnerAsync(id);
            if (test == null)
                return NotFound(new { Message = "Test not found or inactive." });

            return Ok(test);
        }

        // GET api/learner/tests
        [HttpGet]
        public async Task<IActionResult> GetAllActiveTests()
        {
            var tests = await _testService.GetAllActiveTestsAsync();
            return Ok(tests);
        }
    }
}
