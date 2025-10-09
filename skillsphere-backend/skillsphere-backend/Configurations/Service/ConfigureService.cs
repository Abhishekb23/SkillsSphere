
using Microsoft.Extensions.DependencyInjection;
using skillsphere.core.Interfaces.Repositories;
using skillsphere.core.Interfaces.Services;
using skillsphere.infrastructure;
using skillsphere.infrastructure.Data;
using skillsphere.infrastructure.Repositories;
using skillsphere.infrastructure.Services;

namespace EvaluateSystems.Configuration;
public static class ApplicationServiceExtension
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<DatabaseContext>();
        services.AddAutoMapper(typeof(MappingProfile).Assembly);


        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITestRepository, TestRepository>();
        services.AddScoped<ITestService, TestService>();
        return services;
    }
}

