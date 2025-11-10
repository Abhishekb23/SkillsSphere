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


    // ✅ Learner-safe models
    public class LearnerTestDto
    {
        public int TestId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public List<LearnerQuestionDto>? Questions { get; set; }
    }

    public class LearnerTestListDto
    {
        public int TestId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
    }


    public class LearnerQuestionDto
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = null!;
        public string QuestionType { get; set; } = "SingleChoice";
        public List<LearnerOptionDto>? Options { get; set; }
    }

    public class LearnerOptionDto
    {
        public int OptionId { get; set; }
        public string OptionText { get; set; } = null!;
    }

    public class UpdateTestRequest
    {
        public int TestId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public List<UpdateQuestionRequest> Questions { get; set; } = new();
    }

    public class UpdateQuestionRequest
    {
        public int? QuestionId { get; set; } // null = new question
        public string QuestionText { get; set; } = null!;
        public string QuestionType { get; set; } = "SingleChoice";
        public List<UpdateOptionRequest> Options { get; set; } = new();
    }

    public class UpdateOptionRequest
    {
        public int? OptionId { get; set; } // null = new option
        public string OptionText { get; set; } = null!;
        public bool IsCorrect { get; set; }
    }

}
