using System;
using System.Collections.Generic;

namespace skillsphere.core.Dtos
{
    // Request DTOs
    public class CreateCourseRequest
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? ThumbnailUrl { get; set; }
        public int? CreatedBy { get; set; }
        public List<CreateModuleRequest>? Modules { get; set; }
    }

    public class CreateModuleRequest
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int OrderIndex { get; set; } = 0;
        public List<CreateLessonRequest>? Lessons { get; set; }
    }

    public class CreateLessonRequest
    {
        public string Title { get; set; } = null!;
        public int OrderIndex { get; set; } = 0;
        public List<CreateLessonStepRequest>? Steps { get; set; }
    }

    public class CreateLessonStepRequest
    {
        public string? TextContent { get; set; }
        public string? ImageUrl { get; set; }
        public string? ResourceLink { get; set; }
        public int OrderIndex { get; set; } = 0;
    }

    // Response DTOs
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
        public int OrderIndex { get; set; }
        public List<LessonStepDto>? Steps { get; set; } // Include steps
    }

    public class LessonStepDto
    {
        public int LessonStepId { get; set; }
        public string? TextContent { get; set; }
        public string? ImageUrl { get; set; }
        public string? ResourceLink { get; set; }
        public int OrderIndex { get; set; }
    }
}
