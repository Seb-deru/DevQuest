using DevQuest.Application.Identity.GetUserById;
using Microsoft.AspNetCore.Mvc;

namespace DevQuest.HttpApi.Controllers.Identity;
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    [HttpGet]
    public GetUserByIdResponse Get([FromQuery] Guid userId)
    {
        var handler = new GetUserByIdQueryHandler();
        var response = handler.Handle(userId);
        return response;
    }
}
