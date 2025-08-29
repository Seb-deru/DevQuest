using DevQuest.Application.Identity.GetUserById;
using Microsoft.AspNetCore.Mvc;

namespace DevQuest.HttpApi.Controllers.Identity;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IGetUserByIdQueryHandler _getUserByIdHandler;

    public UserController(IGetUserByIdQueryHandler getUserByIdHandler)
    {
        _getUserByIdHandler = getUserByIdHandler;
    }

    [HttpGet]
    public async Task<ActionResult<GetUserByIdResponse>> Get([FromQuery] Guid userId)
    {
        var response = await _getUserByIdHandler.Handle(userId);
        
        if (response == null)
            return NotFound();

        return Ok(response);
    }
}
