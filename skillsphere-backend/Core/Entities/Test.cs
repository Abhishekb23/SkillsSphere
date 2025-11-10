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
        public string? CreatedByName { get; set; } // ✅ Added
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




    public class UserAnswer
    {
        public int UserAnswerId { get; set; }
        public int UserId { get; set; }
        public int TestId { get; set; }
        public int QuestionId { get; set; }
        public List<int> SelectedOptionIds { get; set; } = new();
        public DateTime SubmittedAt { get; set; }
    }

    public class TestResult
    {
        public int ResultId { get; set; }
        public int TestId { get; set; }
        public string TestTitle { get; set; } = null!;
        public int UserId { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public decimal Score { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime CompletedAt { get; set; }
    }

}
