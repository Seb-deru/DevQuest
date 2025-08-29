using DevQuest.Application.Identity;
using DevQuest.Infrastructure.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DevQuest.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IUserRepository, InMemoryUserRepository>();

        return services;
    }
}