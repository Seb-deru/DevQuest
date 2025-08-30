using DevQuest.Application.Identity.CreateUser;
using DevQuest.Application.Identity.GetUserById;
using Microsoft.Extensions.DependencyInjection;

namespace DevQuest.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IGetUserByIdQueryHandler, GetUserByIdQueryHandler>();
        services.AddScoped<ICreateUserCommandHandler, CreateUserCommandHandler>();

        return services;
    }
}