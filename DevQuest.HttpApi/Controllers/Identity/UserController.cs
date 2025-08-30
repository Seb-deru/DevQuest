using DevQuest.Application.Identity.CreateUser;
using DevQuest.Application.Identity.GetUserById;
using Microsoft.AspNetCore.Mvc;

namespace DevQuest.HttpApi.Controllers.Identity;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IGetUserByIdQueryHandler _getUserByIdHandler;
    private readonly ICreateUserCommandHandler _createUserHandler;

    public UserController(
        IGetUserByIdQueryHandler getUserByIdHandler,
        ICreateUserCommandHandler createUserHandler)
    {
        _getUserByIdHandler = getUserByIdHandler;
        _createUserHandler = createUserHandler;
    }

    [HttpGet]
    public async Task<ActionResult<GetUserByIdResponse>> Get([FromQuery] Guid userId)
    {
        var response = await _getUserByIdHandler.Handle(userId);
        
        if (response == null)
            return NotFound();

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<CreateUserResponse>> Create([FromBody] CreateUserRequest request)
    {
        try
        {
            var response = await _createUserHandler.Handle(request);
            return CreatedAtAction(nameof(Get), new { userId = response.Id }, response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }
}
