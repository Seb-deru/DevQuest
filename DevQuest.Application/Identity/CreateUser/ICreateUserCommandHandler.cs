namespace DevQuest.Application.Identity.CreateUser;

public interface ICreateUserCommandHandler
{
    Task<CreateUserResponse> Handle(CreateUserRequest request);
}