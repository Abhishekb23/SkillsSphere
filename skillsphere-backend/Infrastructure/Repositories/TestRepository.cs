using Dapper;
using skillsphere.core.Dtos;
using skillsphere.core.Entities;
using skillsphere.core.Interfaces.Repositories;
using skillsphere.infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        // Create test using PostgreSQL function
        public async Task<int> CreateTestAsync(CreateTestRequest request)
        {
            using var conn = GetConnection();
            conn.Open();

            // Convert DTO to JSON
            var json = System.Text.Json.JsonSerializer.Serialize(new
            {
                title = request.Title,
                description = request.Description,
                createdBy = 1, // Replace with actual admin user id
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

            // Call the PostgreSQL function
            var testId = await conn.ExecuteScalarAsync<int>(
                "SELECT create_test(@JsonData::json);",
                new { JsonData = json }
            );

            return testId;
        }

        // Get test by id (optional)
        public async Task<Test?> GetTestByIdAsync(int testId)
        {
            using var conn = GetConnection();
            conn.Open();

            // Get test
            var test = await conn.QueryFirstOrDefaultAsync<Test>(
                "SELECT * FROM Test WHERE TestId = @Id;",
                new { Id = testId }
            );

            if (test == null) return null;

            // Get questions
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

        // Get all tests (optional)
        public async Task<IEnumerable<Test>> GetAllTestsAsync()
        {
            using var conn = GetConnection();
            conn.Open();
            var tests = await conn.QueryAsync<Test>("SELECT * FROM Test;");
            return tests;
        }
    }

}
