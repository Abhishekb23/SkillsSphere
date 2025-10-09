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
    }

}
