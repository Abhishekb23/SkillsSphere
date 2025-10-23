using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using skillsphere.core.Dtos;
using skillsphere.core.Interfaces.Services;

[ApiController]
[Route("api/admin/courses")]
//[Authorize(Roles = "Admin")]
public class AdminCourseController : ControllerBase
{
    private readonly ICourseService _service;
    public AdminCourseController(ICourseService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequest request)
    {
        var id = await _service.CreateCourseAsync(request);
        return CreatedAtAction(nameof(GetCourse), new { id }, new { CourseId = id });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourse(int id)
    {
        var course = await _service.GetCourseByIdAsync(id);
        if (course == null) return NotFound();
        return Ok(course);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCourse(int id, [FromBody] CreateCourseRequest request)
    {
        await _service.UpdateCourseAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        await _service.DeleteCourseAsync(id);
        return NoContent();
    }

    [HttpPost("{courseId}/modules")]
    public async Task<IActionResult> CreateModule(int courseId, [FromBody] CreateModuleRequest request)
    {
        var moduleId = await _service.CreateModuleAsync(courseId, request);
        return CreatedAtAction(nameof(GetCourse), new { id = courseId }, new { ModuleId = moduleId });
    }

    [HttpPost("modules/{moduleId}/lessons")]
    public async Task<IActionResult> CreateLesson(int moduleId, [FromBody] CreateLessonRequest request)
    {
        var lessonId = await _service.CreateLessonAsync(moduleId, request);
        return Created("", new { LessonId = lessonId });
    }
}
