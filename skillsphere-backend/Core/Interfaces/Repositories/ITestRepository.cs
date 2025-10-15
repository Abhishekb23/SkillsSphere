using skillsphere.core.Dtos;
using skillsphere.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skillsphere.core.Interfaces.Repositories
{
    public interface ITestRepository
    {
        Task<int> CreateTestAsync(CreateTestRequest request);
        Task<Test?> GetTestByIdAsync(int testId);
        Task<IEnumerable<Test>> GetAllTestsAsync();
        Task<IEnumerable<TestResult>> GetUserResultsAsync(int userId);
        Task SubmitAnswersAsync(SubmitTestRequest request);
        Task InsertTestResultAsync(int userId, int testId, int totalQuestions, double correctAnswers, double score);
    }

}
