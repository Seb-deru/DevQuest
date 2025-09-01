using DevQuest.Application.Identity.CreateUser;
using DevQuest.Application.Identity.GetUserById;
using DevQuest.Application.WorkItems.CreateWorkItem;
using DevQuest.Application.WorkItems.GetAllWorkItems;
using DevQuest.Application.WorkItems.GetWorkItemById;
using DevQuest.Application.WorkItems.UpdateWorkItem;
using DevQuest.Application.WorkItems.DeleteWorkItem;
using Microsoft.Extensions.DependencyInjection;

namespace DevQuest.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // User services
        services.AddScoped<IGetUserByIdQueryHandler, GetUserByIdQueryHandler>();
        services.AddScoped<ICreateUserCommandHandler, CreateUserCommandHandler>();

        // WorkItem services
        services.AddScoped<ICreateWorkItemCommandHandler, CreateWorkItemCommandHandler>();
        services.AddScoped<IGetWorkItemByIdQueryHandler, GetWorkItemByIdQueryHandler>();
        services.AddScoped<IGetAllWorkItemsQueryHandler, GetAllWorkItemsQueryHandler>();
        services.AddScoped<IUpdateWorkItemCommandHandler, UpdateWorkItemCommandHandler>();
        services.AddScoped<IDeleteWorkItemCommandHandler, DeleteWorkItemCommandHandler>();

        return services;
    }
}