using skillsphere.core.Dtos;
using skillsphere.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skillsphere.core.Interfaces.Services
{
    public interface ITestService
    {
        Task<int> CreateTestAsync(CreateTestRequest request);
        Task<Test?> GetTestByIdAsync(int testId);
        Task<IEnumerable<Test>> GetAllTestsAsync();
        Task SubmitTestAsync(SubmitTestRequest request);
        Task<IEnumerable<TestResult>> GetUserResultsAsync(int userId);
        Task<int> GetTestsCount();
    }

}
