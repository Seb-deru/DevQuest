using DevQuest.Application.WorkItems.CreateWorkItem;
using DevQuest.Application.WorkItems.GetWorkItemById;
using DevQuest.Application.WorkItems.GetAllWorkItems;
using DevQuest.Application.WorkItems.UpdateWorkItem;
using DevQuest.Application.WorkItems.DeleteWorkItem;
using Microsoft.AspNetCore.Mvc;

namespace DevQuest.HttpApi.Controllers.WorkItems;

[ApiController]
[Route("[controller]")]
public class WorkItemController : ControllerBase
{
    private readonly ICreateWorkItemCommandHandler _createWorkItemHandler;
    private readonly IGetWorkItemByIdQueryHandler _getWorkItemByIdHandler;
    private readonly IGetAllWorkItemsQueryHandler _getAllWorkItemsHandler;
    private readonly IUpdateWorkItemCommandHandler _updateWorkItemHandler;
    private readonly IDeleteWorkItemCommandHandler _deleteWorkItemHandler;

    public WorkItemController(
        ICreateWorkItemCommandHandler createWorkItemHandler,
        IGetWorkItemByIdQueryHandler getWorkItemByIdHandler,
        IGetAllWorkItemsQueryHandler getAllWorkItemsHandler,
        IUpdateWorkItemCommandHandler updateWorkItemHandler,
        IDeleteWorkItemCommandHandler deleteWorkItemHandler)
    {
        _createWorkItemHandler = createWorkItemHandler;
        _getWorkItemByIdHandler = getWorkItemByIdHandler;
        _getAllWorkItemsHandler = getAllWorkItemsHandler;
        _updateWorkItemHandler = updateWorkItemHandler;
        _deleteWorkItemHandler = deleteWorkItemHandler;
    }

    [HttpGet]
    public async Task<ActionResult<GetAllWorkItemsResponse>> GetAll()
    {
        var response = await _getAllWorkItemsHandler.Handle();
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetWorkItemByIdResponse>> GetById(Guid id)
    {
        var response = await _getWorkItemByIdHandler.Handle(id);
        
        if (response == null)
            return NotFound();

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<CreateWorkItemResponse>> Create([FromBody] CreateWorkItemRequest request)
    {
        try
        {
            var response = await _createWorkItemHandler.Handle(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UpdateWorkItemResponse>> Update(Guid id, [FromBody] UpdateWorkItemRequest request)
    {
        try
        {
            // Ensure the ID in the route matches the ID in the request
            if (id != request.Id)
                return BadRequest(new { error = "ID in route does not match ID in request body" });

            var response = await _updateWorkItemHandler.Handle(request);
            
            if (response == null)
                return NotFound();

            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var success = await _deleteWorkItemHandler.Handle(id);
        
        if (!success)
            return NotFound();

        return NoContent();
    }
}