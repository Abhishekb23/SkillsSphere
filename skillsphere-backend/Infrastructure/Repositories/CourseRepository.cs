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

        public async Task<int> CreateCourseAsync(CreateCourseRequest request)
        {
            using var conn = GetConnection();
            conn.Open();

            var courseId = await conn.ExecuteScalarAsync<int>(
                @"INSERT INTO ""Course"" (""Title"", ""Description"", ""ThumbnailUrl"", ""CreatedBy"", ""IsActive"", ""CreatedAt"")
              VALUES (@Title, @Description, @ThumbnailUrl, @CreatedBy, @IsActive, NOW())
              RETURNING ""CourseId"";",
                new { request.Title, request.Description, request.ThumbnailUrl, request.CreatedBy, IsActive = true });

            // optional: create modules + lessons if provided
            if (request.Modules != null)
            {
                foreach (var mod in request.Modules)
                {
                    var moduleId = await CreateModuleAsync(courseId, mod);
                    if (mod.Lessons != null)
                    {
                        foreach (var lesson in mod.Lessons)
                        {
                            await CreateLessonAsync(moduleId, lesson);
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

            var imageJson = JsonSerializer.Serialize(request.ImageUrls ?? new List<string>());
            var linksJson = JsonSerializer.Serialize(request.ResourceLinks ?? new List<string>());

            var lessonId = await conn.ExecuteScalarAsync<int>(
                @"INSERT INTO ""Lesson"" (""ModuleId"", ""Title"", ""Content"", ""ImageUrls"", ""ResourceLinks"", ""OrderIndex"", ""CreatedAt"")
              VALUES (@ModuleId, @Title, @Content, @ImageUrls::jsonb, @ResourceLinks::jsonb, @OrderIndex, NOW())
              RETURNING ""LessonId"";",
                new
                {
                    ModuleId = moduleId,
                    request.Title,
                    request.Content,
                    ImageUrls = imageJson,
                    ResourceLinks = linksJson,
                    request.OrderIndex
                });

            return lessonId;
        }

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
                var lessons = await conn.QueryAsync<dynamic>(
                    @"SELECT * FROM ""Lesson"" WHERE ""ModuleId"" = @MId ORDER BY ""OrderIndex"";",
                    new { MId = module.ModuleId });

                var lessonList = new List<Lesson>();
                foreach (var l in lessons)
                {
                    // map dynamic to Lesson and parse JSONB
                    var lesson = new Lesson
                    {
                        LessonId = l.lessonid,
                        ModuleId = l.moduleid,
                        Title = l.title,
                        Content = l.content,
                        OrderIndex = l.orderindex,
                        CreatedAt = l.createdat
                    };
                    // parse JSONB fields
                    lesson.ImageUrls = l.imageurls == null ? new List<string>() : JsonSerializer.Deserialize<List<string>>(l.imageurls.ToString());
                    lesson.ResourceLinks = l.resourcelinks == null ? new List<string>() : JsonSerializer.Deserialize<List<string>>(l.resourcelinks.ToString());
                    lessonList.Add(lesson);
                }

                module.Lessons = lessonList;
            }

            course.Modules = modules.ToList();
            return course;
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync(bool onlyActive = true)
        {
            using var conn = GetConnection();
            conn.Open();

            if (onlyActive)
            {
                var tests = await conn.QueryAsync<Course>(@"SELECT * FROM ""Course"" WHERE ""IsActive"" = true ORDER BY ""CreatedAt"" DESC;");
                return tests;
            }
            else
            {
                var tests = await conn.QueryAsync<Course>(@"SELECT * FROM ""Course"" ORDER BY ""CreatedAt"" DESC;");
                return tests;
            }
        }

        public async Task DeleteCourseAsync(int courseId)
        {
            using var conn = GetConnection();
            conn.Open();

            await conn.ExecuteAsync(@"DELETE FROM ""Course"" WHERE ""CourseId"" = @Id;", new { Id = courseId });
        }

        public async Task UpdateCourseAsync(int courseId, CreateCourseRequest request)
        {
            using var conn = GetConnection();
            conn.Open();

            await conn.ExecuteAsync(
                @"UPDATE ""Course"" SET ""Title"" = @Title, ""Description"" = @Description, ""ThumbnailUrl"" = @ThumbnailUrl, ""IsActive"" = @IsActive
              WHERE ""CourseId"" = @CourseId;",
                new { request.Title, request.Description, request.ThumbnailUrl, IsActive = true, CourseId = courseId });
            // Modules/lessons update endpoints can be implemented separately
        }
    }

}
