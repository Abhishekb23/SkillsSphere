using Dapper;
using skillsphere.core.Dtos;
using skillsphere.core.Entities;
using skillsphere.core.Interfaces.Repositories;
using skillsphere.infrastructure.Data;
using System.Data;

namespace skillsphere.infrastructure.Repositories
{
    public class TestRepository : ITestRepository
    {
        private readonly DatabaseContext _dbContext;

        public TestRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        private IDbConnection GetConnection() => _dbContext.CreateConnection();

        // -----------------------------
        // CREATE TEST (uses create_test function)
        // -----------------------------
        public async Task<int> CreateTestAsync(CreateTestRequest request)
        {
            using var conn = GetConnection();
            conn.Open();

            var json = System.Text.Json.JsonSerializer.Serialize(new
            {
                title = request.Title,
                description = request.Description,
                createdBy = 1,
                questions = request.Questions.Select(q => new
                {
                    questionText = q.QuestionText,
                    questionType = q.QuestionType,
                    options = q.Options.Select(o => new
                    {
                        optionText = o.OptionText,
                        isCorrect = o.IsCorrect
                    })
                })
            });

            return await conn.ExecuteScalarAsync<int>(
                "SELECT create_test(@JsonData::json)",
                new { JsonData = json }
            );
        }


        // -----------------------------
        // UPDATE TEST (uses update_test function)
        // -----------------------------
        public async Task<bool> UpdateTestAsync(UpdateTestRequest request)
        {
            using var conn = GetConnection();
            conn.Open();

            var json = System.Text.Json.JsonSerializer.Serialize(new
            {
                testId = request.TestId,
                title = request.Title,
                description = request.Description,
                isActive = request.IsActive,
                questions = request.Questions.Select(q => new
                {
                    questionId = q.QuestionId,
                    questionText = q.QuestionText,
                    questionType = q.QuestionType,
                    options = q.Options.Select(o => new
                    {
                        optionId = o.OptionId,
                        optionText = o.OptionText,
                        isCorrect = o.IsCorrect
                    })
                })
            });

            var result = await conn.ExecuteScalarAsync<int>(
                "SELECT update_test(@JsonData::json)",
                new { JsonData = json }
            );

            return result == 1;
        }


        // -----------------------------
        // GET TEST BY ID
        // (Keeping your logic the same)
        // -----------------------------
        public async Task<Test?> GetTestByIdAsync(int testId)
        {
            using var conn = GetConnection();
            conn.Open();

            var test = await conn.QueryFirstOrDefaultAsync<Test>(
                "SELECT * FROM Test WHERE TestId = @Id;",
                new { Id = testId }
            );

            if (test == null) return null;

            var questions = await conn.QueryAsync<Question>(
                "SELECT * FROM Question WHERE TestId = @Id;",
                new { Id = testId }
            );

            foreach (var question in questions)
            {
                var options = await conn.QueryAsync<Option>(
                    "SELECT * FROM Option WHERE QuestionId = @QId;",
                    new { QId = question.QuestionId }
                );
                question.Options = options.ToList();
            }

            test.Questions = questions.ToList();
            return test;
        }


        // -----------------------------
        // GET ALL TESTS
        // -----------------------------
        public async Task<IEnumerable<Test>> GetAllTestsAsync()
        {
            using var conn = GetConnection();
            conn.Open();

            return await conn.QueryAsync<Test>("SELECT * FROM Test;");
        }


        // -----------------------------
        // SUBMIT ANSWERS (convert to SP)
        // -----------------------------
        public async Task SubmitAnswersAsync(SubmitTestRequest request)
        {
            using var conn = GetConnection();
            conn.Open();

            var json = System.Text.Json.JsonSerializer.Serialize(request.Answers);

            await conn.ExecuteAsync(
                "CALL submit_user_answers(@UserId, @TestId, @JsonData::json)",
                new { request.UserId, request.TestId, JsonData = json }
            );
        }


        // -----------------------------
        // GET USER RESULTS
        // -----------------------------
        public async Task<IEnumerable<TestResult>> GetUserResultsAsync(int userId)
        {
            using var conn = GetConnection();
            conn.Open();

            return await conn.QueryAsync<TestResult>(
                @"SELECT * FROM ""TestResult"" WHERE ""UserId"" = @UserId",
                new { UserId = userId }
            );
        }


        // -----------------------------
        // INSERT TEST RESULT (SP)
        // -----------------------------
        public async Task InsertTestResultAsync(int userId, int testId, int totalQuestions, double correctAnswers, double score)
        {
            using var conn = GetConnection();
            conn.Open();

            await conn.ExecuteAsync(
                "CALL insert_test_result(@userId, @testId, @total, @correct, @score)",
                new
                {
                    userId,
                    testId,
                    total = totalQuestions,
                    correct = correctAnswers,
                    score
                });
        }


        // -----------------------------
        // GET TEST COUNT (function)
        // -----------------------------
        public async Task<int> GetTestsCount()
        {
            using var conn = GetConnection();
            conn.Open();

            return await conn.ExecuteScalarAsync<int>("SELECT get_tests_count()");
        }


        // -----------------------------
        // DELETE TEST (SP)
        // -----------------------------
        public async Task DeleteTestAsync(int testId)
        {
            using var conn = GetConnection();
            conn.Open();

            await conn.ExecuteAsync("CALL delete_test(@Id)", new { Id = testId });
        }


        // -----------------------------
        // ADD THUMBNAIL (SP)
        // -----------------------------
        public async Task AddThumbnailAsync(int testId, byte[] imageData)
        {
            using var conn = GetConnection();
            conn.Open();

            await conn.ExecuteAsync(
                "CALL add_thumbnail(@TestId, @ImageData)",
                new { TestId = testId, ImageData = imageData }
            );
        }


        // -----------------------------
        // GET THUMBNAIL (function)
        // -----------------------------
        public async Task<byte[]?> GetThumbnailAsync(int testId)
        {
            using var conn = GetConnection();
            conn.Open();

            return await conn.ExecuteScalarAsync<byte[]?>(
                "SELECT get_thumbnail(@TestId)",
                new { TestId = testId }
            );
        }

        public async Task DeleteThumbnailAsync(int testId)
        {
            using var conn = GetConnection();
            conn.Open();

            await conn.ExecuteAsync(
                "DELETE FROM TestThumbnail WHERE TestId = @TestId",
                new { TestId = testId }
            );
        }

    }
}
