
namespace DevQuest.Application.Identity.GetUserById;

public interface IGetUserByIdQueryHandler
{
    Task<GetUserByIdResponse?> Handle(Guid userId);
}