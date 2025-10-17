using System;
using System.Collections.Generic;

namespace skillsphere.core.Entities
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? ThumbnailUrl { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public List<Module>? Modules { get; set; }
    }

    public class Module
    {
        public int ModuleId { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int OrderIndex { get; set; } = 0;
        public List<Lesson>? Lessons { get; set; }
    }

    public class Lesson
    {
        public int LessonId { get; set; }
        public int ModuleId { get; set; }
        public string Title { get; set; } = null!;
        public int OrderIndex { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
        public List<LessonStep>? Steps { get; set; } // New: step-by-step content
    }

    public class LessonStep
    {
        public int LessonStepId { get; set; }
        public int LessonId { get; set; }
        public string? TextContent { get; set; }  // Text explanation
        public string? ImageUrl { get; set; }     // Optional screenshot/image
        public string? ResourceLink { get; set; } // Optional link
        public int OrderIndex { get; set; } = 0;  // Step order
    }
}
