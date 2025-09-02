using DevQuest.Application.WorkItems.CreateWorkItem;
using DevQuest.Application.WorkItems.DeleteWorkItem;
using DevQuest.Application.WorkItems.GetAllWorkItems;
using DevQuest.Application.WorkItems.GetWorkItemById;
using DevQuest.Application.WorkItems.UpdateWorkItem;

namespace DevQuest.BlazorApp.Services;

public class WorkItemService : IWorkItemService
{
    private readonly IGetAllWorkItemsQueryHandler _getAllHandler;
    private readonly IGetWorkItemByIdQueryHandler _getByIdHandler;
    private readonly ICreateWorkItemCommandHandler _createHandler;
    private readonly IUpdateWorkItemCommandHandler _updateHandler;
    private readonly IDeleteWorkItemCommandHandler _deleteHandler;

    public WorkItemService(
        IGetAllWorkItemsQueryHandler getAllHandler,
        IGetWorkItemByIdQueryHandler getByIdHandler,
        ICreateWorkItemCommandHandler createHandler,
        IUpdateWorkItemCommandHandler updateHandler,
        IDeleteWorkItemCommandHandler deleteHandler)
    {
        _getAllHandler = getAllHandler;
        _getByIdHandler = getByIdHandler;
        _createHandler = createHandler;
        _updateHandler = updateHandler;
        _deleteHandler = deleteHandler;
    }

    public async Task<GetAllWorkItemsResponse> GetAllAsync()
    {
        return await _getAllHandler.Handle();
    }

    public async Task<GetWorkItemByIdResponse?> GetByIdAsync(Guid id)
    {
        return await _getByIdHandler.Handle(id);
    }

    public async Task<CreateWorkItemResponse> CreateAsync(CreateWorkItemRequest request)
    {
        return await _createHandler.Handle(request);
    }

    public async Task<UpdateWorkItemResponse> UpdateAsync(UpdateWorkItemRequest request)
    {
        return await _updateHandler.Handle(request);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _deleteHandler.Handle(id);
    }
}