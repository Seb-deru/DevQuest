using DevQuest.Application.Identity;
using DevQuest.Application.WorkItems;
using DevQuest.Infrastructure.Identity;
using DevQuest.Infrastructure.WorkItems;
using Microsoft.Extensions.DependencyInjection;

namespace DevQuest.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Identity services
        services.AddSingleton<IUserRepository, InMemoryUserRepository>();
        services.AddScoped<IPasswordHashingService, PasswordHashingService>();

        // WorkItem services
        services.AddSingleton<IWorkItemRepository, InMemoryWorkItemRepository>();

        return services;
    }
}