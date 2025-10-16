using skillsphere.core.Dtos;
using skillsphere.core.Entities;
using skillsphere.core.Interfaces.Repositories;
using skillsphere.core.Interfaces.Services;

namespace skillsphere.infrastructure.Services
{
    public class TestService : ITestService
    {
        private readonly ITestRepository _testRepository;

        public TestService(ITestRepository testRepository)
        {
            _testRepository = testRepository;
        }

        public async Task<int> CreateTestAsync(CreateTestRequest request)
        {
            // You can add extra validation here if needed
            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ArgumentException("Test title is required");

            if (request.Questions == null || !request.Questions.Any())
                throw new ArgumentException("At least one question is required");

            return await _testRepository.CreateTestAsync(request);
        }

        public async Task<Test?> GetTestByIdAsync(int testId)
        {
            return await _testRepository.GetTestByIdAsync(testId);
        }

        public async Task<IEnumerable<Test>> GetAllTestsAsync()
        {
            return await _testRepository.GetAllTestsAsync();
        }


        public async Task SubmitTestAsync(SubmitTestRequest request)
        {
            // Step 1: Insert user answers
            await _testRepository.SubmitAnswersAsync(request);

            // Step 2: Get full test with questions and options
            var test = await _testRepository.GetTestByIdAsync(request.TestId);
            if (test == null)
                throw new Exception("Test not found");

            int totalQuestions = test.Questions?.Count ?? 0;
            double correctAnswersCount = 0;

            foreach (var question in test.Questions!)
            {
                // Get user's answer for this question
                var userAnswer = request.Answers.FirstOrDefault(a => a.QuestionId == question.QuestionId);
                if (userAnswer == null) continue;

                // Correct option IDs
                var correctOptionIds = question.Options?
                    .Where(o => o.IsCorrect)
                    .Select(o => o.OptionId)
                    .ToList() ?? new List<int>();

                var userOptionIds = userAnswer.SelectedOptionIds ?? new List<int>();

                if (!correctOptionIds.Any()) continue;

                if (question.QuestionType == "SingleChoice")
                {
                    if (correctOptionIds.SequenceEqual(userOptionIds))
                        correctAnswersCount += 1;
                }
                else if (question.QuestionType == "MultipleChoice")
                {
                    // If user selected any wrong option, score = 0
                    bool hasWrongSelection = userOptionIds.Any(u => !correctOptionIds.Contains(u));
                    if (hasWrongSelection)
                    {
                        correctAnswersCount += 0;
                    }
                    else
                    {
                        // Fractional score for correct selections only
                        int matched = userOptionIds.Count(u => correctOptionIds.Contains(u));
                        double fractionScore = (double)matched / correctOptionIds.Count;
                        correctAnswersCount += fractionScore;
                    }
                }
            }

            // Step 3: Calculate score %
            double scorePercent = totalQuestions > 0
                ? (correctAnswersCount / totalQuestions) * 100
                : 0;

            // Step 4: Insert into TestResult
            await _testRepository.InsertTestResultAsync(
                request.UserId,
                request.TestId,
                totalQuestions,
                Math.Round(correctAnswersCount, 2), // store as double
                Math.Round(scorePercent, 2)
            );
        }


        public async Task<IEnumerable<TestResult>> GetUserResultsAsync(int userId)
        {
            return await _testRepository.GetUserResultsAsync(userId);
        }

        public async Task<int> GetTestsCount()
        {
            return await _testRepository.GetTestsCount();
        }
    }

}
