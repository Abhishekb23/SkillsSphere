using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skillsphere.core.Dtos
{
    public class CreateTestRequest
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public List<CreateQuestionRequest> Questions { get; set; } = new();
    }

    public class CreateQuestionRequest
    {
        public string QuestionText { get; set; } = null!;
        public string QuestionType { get; set; } = "SingleChoice";
        public List<CreateOptionRequest> Options { get; set; } = new();
    }

    public class CreateOptionRequest
    {
        public string OptionText { get; set; } = null!;
        public bool IsCorrect { get; set; }
    }

    public class CreateTestResponse
    {
        public int TestId { get; set; }
        public string Title { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }

    public class SubmitTestRequest
    {
        public int TestId { get; set; }
        public int UserId { get; set; }
        public List<UserAnswerDto> Answers { get; set; } = new();
    }

    public class UserAnswerDto
    {
        public int QuestionId { get; set; }
        public List<int> SelectedOptionIds { get; set; } = new();
    }
}
