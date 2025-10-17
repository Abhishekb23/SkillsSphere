using skillsphere.core.Dtos;
using skillsphere.core.Entities;
using skillsphere.core.Interfaces.Repositories;
using skillsphere.core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace skillsphere.infrastructure.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repo;
        public CourseService(ICourseRepository repo) => _repo = repo;

        // -------------------------
        // CREATE COURSE WITH MODULES & LESSONS & STEPS
        // -------------------------
        public async Task<int> CreateCourseAsync(CreateCourseRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ArgumentException("Course title is required");

            if (request.Modules != null)
            {
                foreach (var mod in request.Modules)
                {
                    if (string.IsNullOrWhiteSpace(mod.Title))
                        throw new ArgumentException("Module title is required");

                    if (mod.Lessons != null)
                    {
                        foreach (var lesson in mod.Lessons)
                        {
                            if (string.IsNullOrWhiteSpace(lesson.Title))
                                throw new ArgumentException("Lesson title is required");
                        }
                    }
                }
            }

            return await _repo.CreateCourseAsync(request);
        }

        public Task<Course?> GetCourseByIdAsync(int courseId) => _repo.GetCourseByIdAsync(courseId);

        public Task<IEnumerable<Course>> GetAllCoursesAsync(bool onlyActive = true) => _repo.GetAllCoursesAsync(onlyActive);

        public async Task<int> CreateModuleAsync(int courseId, CreateModuleRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ArgumentException("Module title is required");

            return await _repo.CreateModuleAsync(courseId, request);
        }

        public async Task<int> CreateLessonAsync(int moduleId, CreateLessonRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ArgumentException("Lesson title is required");

            var lessonId = await _repo.CreateLessonAsync(moduleId, request);

            // optional: create lesson steps if provided
            //if (request.Steps != null)
            //{
            //    foreach (var step in request.Steps)
            //    {
            //        if (string.IsNullOrWhiteSpace(step.TextContent))
            //            throw new ArgumentException("Lesson step text is required");

            //        await _repo.CreateLessonStepAsync(lessonId, step);
            //    }
            //}

            return lessonId;
        }

        public Task DeleteCourseAsync(int courseId) => _repo.DeleteCourseAsync(courseId);

        public Task UpdateCourseAsync(int courseId, CreateCourseRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ArgumentException("Course title is required");

            return _repo.UpdateCourseAsync(courseId, request);
        }
    }
}
