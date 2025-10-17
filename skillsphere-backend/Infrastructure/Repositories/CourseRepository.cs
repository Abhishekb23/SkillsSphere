using Dapper;
using System.Data;
using System.Text.Json;
using skillsphere.core.Dtos;
using skillsphere.core.Entities;
using skillsphere.core.Interfaces.Repositories;
using skillsphere.infrastructure.Data;

namespace skillsphere.infrastructure.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly DatabaseContext _dbContext;
        public CourseRepository(DatabaseContext dbContext) => _dbContext = dbContext;
        private IDbConnection GetConnection() => _dbContext.CreateConnection();

        // -------------------------
        // CREATE COURSE + MODULES + LESSONS + STEPS
        // -------------------------
        public async Task<int> CreateCourseAsync(CreateCourseRequest request)
        {
            using var conn = GetConnection();
            conn.Open();

            var courseId = await conn.ExecuteScalarAsync<int>(
                @"INSERT INTO ""Course"" (""Title"", ""Description"", ""ThumbnailUrl"", ""CreatedBy"", ""IsActive"", ""CreatedAt"")
                  VALUES (@Title, @Description, @ThumbnailUrl, @CreatedBy, @IsActive, NOW())
                  RETURNING ""CourseId"";",
                new { request.Title, request.Description, request.ThumbnailUrl, request.CreatedBy, IsActive = true });

            if (request.Modules != null)
            {
                foreach (var mod in request.Modules)
                {
                    var moduleId = await CreateModuleAsync(courseId, mod);
                    if (mod.Lessons != null)
                    {
                        foreach (var lesson in mod.Lessons)
                        {
                            var lessonId = await CreateLessonAsync(moduleId, lesson);

                            // create lesson steps
                            if (lesson.Steps != null)
                            {
                                foreach (var step in lesson.Steps)
                                {
                                    await CreateLessonStepAsync(lessonId, step);
                                }
                            }
                        }
                    }
                }
            }

            return courseId;
        }

        public async Task<int> CreateModuleAsync(int courseId, CreateModuleRequest request)
        {
            using var conn = GetConnection();
            conn.Open();

            var moduleId = await conn.ExecuteScalarAsync<int>(
                @"INSERT INTO ""Module"" (""CourseId"", ""Title"", ""Description"", ""OrderIndex"")
                  VALUES (@CourseId, @Title, @Description, @OrderIndex)
                  RETURNING ""ModuleId"";",
                new { CourseId = courseId, request.Title, request.Description, request.OrderIndex });

            return moduleId;
        }

        public async Task<int> CreateLessonAsync(int moduleId, CreateLessonRequest request)
        {
            using var conn = GetConnection();
            conn.Open();

            var lessonId = await conn.ExecuteScalarAsync<int>(
                @"INSERT INTO ""Lesson"" (""ModuleId"", ""Title"", ""OrderIndex"", ""CreatedAt"")
                  VALUES (@ModuleId, @Title, @OrderIndex, NOW())
                  RETURNING ""LessonId"";",
                new { ModuleId = moduleId, request.Title, request.OrderIndex });

            return lessonId;
        }

        public async Task<int> CreateLessonStepAsync(int lessonId, CreateLessonStepRequest request)
        {
            using var conn = GetConnection();
            conn.Open();

            var stepId = await conn.ExecuteScalarAsync<int>(
                @"INSERT INTO ""LessonStep"" (""LessonId"", ""TextContent"", ""ImageUrl"", ""ResourceLink"", ""OrderIndex"")
                  VALUES (@LessonId, @TextContent, @ImageUrl, @ResourceLink, @OrderIndex)
                  RETURNING ""LessonStepId"";",
                new { LessonId = lessonId, request.TextContent, request.ImageUrl, request.ResourceLink, request.OrderIndex });

            return stepId;
        }

        // -------------------------
        // GET COURSE WITH MODULES, LESSONS & STEPS
        // -------------------------
        public async Task<Course?> GetCourseByIdAsync(int courseId)
        {
            using var conn = GetConnection();
            conn.Open();

            var course = await conn.QueryFirstOrDefaultAsync<Course>(
                @"SELECT * FROM ""Course"" WHERE ""CourseId"" = @Id;",
                new { Id = courseId });

            if (course == null) return null;

            var modules = await conn.QueryAsync<Module>(
                @"SELECT * FROM ""Module"" WHERE ""CourseId"" = @Id ORDER BY ""OrderIndex"";",
                new { Id = courseId });

            foreach (var module in modules)
            {
                var lessons = await conn.QueryAsync<Lesson>(
                    @"SELECT * FROM ""Lesson"" WHERE ""ModuleId"" = @MId ORDER BY ""OrderIndex"";",
                    new { MId = module.ModuleId });

                foreach (var lesson in lessons)
                {
                    var steps = await conn.QueryAsync<LessonStep>(
                        @"SELECT * FROM ""LessonStep"" WHERE ""LessonId"" = @LId ORDER BY ""OrderIndex"";",
                        new { LId = lesson.LessonId });

                    lesson.Steps = steps.ToList();
                }

                module.Lessons = lessons.ToList();
            }

            course.Modules = modules.ToList();
            return course;
        }

        // -------------------------
        // GET ALL COURSES
        // -------------------------
        public async Task<IEnumerable<Course>> GetAllCoursesAsync(bool onlyActive = true)
        {
            using var conn = GetConnection();
            conn.Open();

            if (onlyActive)
            {
                return await conn.QueryAsync<Course>(@"SELECT * FROM ""Course"" WHERE ""IsActive"" = true ORDER BY ""CreatedAt"" DESC;");
            }

            return await conn.QueryAsync<Course>(@"SELECT * FROM ""Course"" ORDER BY ""CreatedAt"" DESC;");
        }

        // -------------------------
        // DELETE COURSE
        // -------------------------
        public async Task DeleteCourseAsync(int courseId)
        {
            using var conn = GetConnection();
            conn.Open();
            await conn.ExecuteAsync(@"DELETE FROM ""Course"" WHERE ""CourseId"" = @Id;", new { Id = courseId });
        }

        // -------------------------
        // UPDATE COURSE (modules & lessons separately)
        // -------------------------
        public async Task UpdateCourseAsync(int courseId, CreateCourseRequest request)
        {
            using var conn = GetConnection();
            conn.Open();

            await conn.ExecuteAsync(
                @"UPDATE ""Course"" 
                  SET ""Title"" = @Title, ""Description"" = @Description, ""ThumbnailUrl"" = @ThumbnailUrl, ""IsActive"" = @IsActive
                  WHERE ""CourseId"" = @CourseId;",
                new { request.Title, request.Description, request.ThumbnailUrl, IsActive = true, CourseId = courseId });
        }

    }
}
