namespace DevQuest.Application.Identity.GetUserById;

public class GetUserByIdQueryHandler : IGetUserByIdQueryHandler
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<GetUserByIdResponse?> Handle(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
            return null;

        return new GetUserByIdResponse(user.Id, user.UserName, user.Email);
    }
}
