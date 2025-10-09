using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skillsphere.core.Entities
{
    public class Test
    {
        public int TestId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public List<Question>? Questions { get; set; }
    }

    public class Question
    {
        public int QuestionId { get; set; }
        public int TestId { get; set; }
        public string QuestionText { get; set; } = null!;
        public string QuestionType { get; set; } = "SingleChoice";
        public List<Option>? Options { get; set; }
    }

    public class Option
    {
        public int OptionId { get; set; }
        public int QuestionId { get; set; }
        public string OptionText { get; set; } = null!;
        public bool IsCorrect { get; set; } = false;
    }

}
