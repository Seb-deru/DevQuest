namespace DevQuest.Application.Identity.GetUserById;
public class GetUserByIdQueryHandler
{
    public GetUserByIdResponse Handle(Guid userId)
    {
        return new(userId, "dummy", "dummy@dummy.dummy");
    }
}
