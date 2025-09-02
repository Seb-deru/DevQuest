using DevQuest.Application.WorkItems.CreateWorkItem;
using DevQuest.Application.WorkItems.GetAllWorkItems;
using DevQuest.Application.WorkItems.GetWorkItemById;
using DevQuest.Application.WorkItems.UpdateWorkItem;

namespace DevQuest.BlazorApp.Services;

public interface IWorkItemService
{
    Task<GetAllWorkItemsResponse> GetAllAsync();
    Task<GetWorkItemByIdResponse?> GetByIdAsync(Guid id);
    Task<CreateWorkItemResponse> CreateAsync(CreateWorkItemRequest request);
    Task<UpdateWorkItemResponse> UpdateAsync(UpdateWorkItemRequest request);
    Task DeleteAsync(Guid id);
}