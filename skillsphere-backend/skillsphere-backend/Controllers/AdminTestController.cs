using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using skillsphere.core.Dtos;
using skillsphere.core.Interfaces.Services;

namespace skillsphere_backend.Controllers
{
    [ApiController]
    [Route("api/admin/tests")]
    //[Authorize(Roles = "Admin")]
    public class AdminTestController : ControllerBase
    {
        private readonly ITestService _testService;

        public AdminTestController(ITestService testService)
        {
            _testService = testService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTest([FromBody] CreateTestRequest request)
        {
            try
            {
                var testId = await _testService.CreateTestAsync(request);
                return Ok(new { TestId = testId, Message = "Test created successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }

        // GET api/admin/tests/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTest(int id)
        {
            var test = await _testService.GetTestByIdAsync(id);
            if (test == null) return NotFound(new { Message = "Test not found" });
            return Ok(test);
        }

        // GET api/admin/tests
        [HttpGet]
        public async Task<IActionResult> GetAllTests()
        {
            var tests = await _testService.GetAllTestsAsync();
            return Ok(tests);
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetTestsCount()
        {
            var tests = await _testService.GetTestsCount();
            return Ok(tests);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTest(int id)
        {
            await _testService.DeleteTestAsync(id);
            return NoContent();
        }


        [HttpPost("{testId}/thumbnail")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadTestThumbnail(int testId, IFormFile file)
        {
            try
            {
                await _testService.AddThumbnailAsync(testId, file);
                return Ok(new { Message = "Thumbnail uploaded successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }

        [HttpGet("{testId}/thumbnail")]
        public async Task<IActionResult> GetTestThumbnail(int testId)
        {
            var result = await _testService.GetThumbnailAsync(testId);
            if (result == null)
                return NotFound(new { Message = "Thumbnail not found" });

            return result; // ✅ Returns image file
        }
    }

}
 