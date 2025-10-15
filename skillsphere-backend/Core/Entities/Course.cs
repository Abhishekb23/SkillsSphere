using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skillsphere.core.Entities
{
    // namespace skillsphere.core.Entities
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
        public string? Content { get; set; } // markdown or html
        public List<string>? ImageUrls { get; set; }
        public List<string>? ResourceLinks { get; set; }
        public int OrderIndex { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
    }

}
