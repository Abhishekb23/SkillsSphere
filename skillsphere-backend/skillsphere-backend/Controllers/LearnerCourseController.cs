using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using skillsphere.core.Interfaces.Services;

[ApiController]
[Route("api/learner/course")]
[Authorize] // allow any authenticated learner
public class LearnerCourseController : ControllerBase
{
    private readonly ICourseService _service;
    public LearnerCourseController(ICourseService service) => _service = service;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var courses = await _service.GetAllCoursesAsync(true);
        return Ok(courses);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var course = await _service.GetCourseByIdAsync(id);
        if (course == null) return NotFound();
        return Ok(course);
    }
}
