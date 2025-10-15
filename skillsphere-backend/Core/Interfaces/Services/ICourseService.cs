using skillsphere.core.Dtos;
using skillsphere.core.Entities;
using skillsphere.core.Interfaces.Repositories;
using skillsphere.core.Interfaces.Services;
namespace skillsphere.core.Interfaces.Services
{
   

    public interface ICourseService
    {
        Task<int> CreateCourseAsync(CreateCourseRequest request);
        Task<Course?> GetCourseByIdAsync(int courseId);
        Task<IEnumerable<Course>> GetAllCoursesAsync(bool onlyActive = true);
        Task<int> CreateModuleAsync(int courseId, CreateModuleRequest request);
        Task<int> CreateLessonAsync(int moduleId, CreateLessonRequest request);
        Task DeleteCourseAsync(int courseId);
        Task UpdateCourseAsync(int courseId, CreateCourseRequest request);
    }

   

}
