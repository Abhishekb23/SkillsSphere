using skillsphere.core.Dtos;
using skillsphere.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skillsphere.core.Interfaces.Repositories
{
    public interface ICourseRepository
    {
        Task<int> CreateCourseAsync(CreateCourseRequest request);
        Task<int> CreateModuleAsync(int courseId, CreateModuleRequest request);
        Task<int> CreateLessonAsync(int moduleId, CreateLessonRequest request);
        Task<Course?> GetCourseByIdAsync(int courseId);
        Task<IEnumerable<Course>> GetAllCoursesAsync(bool onlyActive = true);
        Task DeleteCourseAsync(int courseId);
        Task UpdateCourseAsync(int courseId, CreateCourseRequest request);


    }

}