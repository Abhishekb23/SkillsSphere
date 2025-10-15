using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skillsphere.core.Dtos
{
    // CreateCourseRequest
    public class CreateCourseRequest
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? ThumbnailUrl { get; set; }
        public int? CreatedBy { get; set; }
        public List<CreateModuleRequest>? Modules { get; set; }
    }

    // CreateModuleRequest
    public class CreateModuleRequest
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int OrderIndex { get; set; } = 0;
        public List<CreateLessonRequest>? Lessons { get; set; }
    }

    // CreateLessonRequest
    public class CreateLessonRequest
    {
        public string Title { get; set; } = null!;
        public string? Content { get; set; }
        public List<string>? ImageUrls { get; set; }
        public List<string>? ResourceLinks { get; set; }
        public int OrderIndex { get; set; } = 0;
    }

    // CourseDetailDto - for GET course with modules & lessons
    public class CourseDetailDto
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? ThumbnailUrl { get; set; }
        public bool IsActive { get; set; }
        public List<ModuleDto>? Modules { get; set; }
    }

    public class ModuleDto
    {
        public int ModuleId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int OrderIndex { get; set; }
        public List<LessonDto>? Lessons { get; set; }
    }

    public class LessonDto
    {
        public int LessonId { get; set; }
        public string Title { get; set; } = null!;
        public string? Content { get; set; }
        public List<string>? ImageUrls { get; set; }
        public List<string>? ResourceLinks { get; set; }
        public int OrderIndex { get; set; }
    }

}
