using skillsphere.core.Dtos;
using skillsphere.core.Entities;
using skillsphere.core.Interfaces.Repositories;
using skillsphere.core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skillsphere.infrastructure.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repo;
        public CourseService(ICourseRepository repo) => _repo = repo;

        public async Task<int> CreateCourseAsync(CreateCourseRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title)) throw new ArgumentException("Title required");
            return await _repo.CreateCourseAsync(request);
        }

        public Task<Course?> GetCourseByIdAsync(int courseId) => _repo.GetCourseByIdAsync(courseId);

        public Task<IEnumerable<Course>> GetAllCoursesAsync(bool onlyActive = true) => _repo.GetAllCoursesAsync(onlyActive);

        public Task<int> CreateModuleAsync(int courseId, CreateModuleRequest request) => _repo.CreateModuleAsync(courseId, request);

        public Task<int> CreateLessonAsync(int moduleId, CreateLessonRequest request) => _repo.CreateLessonAsync(moduleId, request);

        public Task DeleteCourseAsync(int courseId) => _repo.DeleteCourseAsync(courseId);

        public Task UpdateCourseAsync(int courseId, CreateCourseRequest request) => _repo.UpdateCourseAsync(courseId, request);
    }
}
