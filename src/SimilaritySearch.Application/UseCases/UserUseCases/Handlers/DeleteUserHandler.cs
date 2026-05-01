using StockMapSvelte.Application.Abstractions;
using StockMapSvelte.Application.Abstractions.Repositories;
using StockMapSvelte.Application.Exceptions.User;
using StockMapSvelte.Application.UseCases.UserUseCases.Commands;

namespace StockMapSvelte.Application.UseCases.UserUseCases.Handlers;

public class DeleteUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;

    public DeleteUserHandler(IUserRepository userRepository, IUserContext userContext)
    {
        _userRepository = userRepository;
        _userContext = userContext;
    }
    
    public async Task Handle(DeleteUserCommand cmd)
    {
        var currentUserId = await _userContext.GetCurrentUserIdAsync();
        if (currentUserId == cmd.UserId)
        {
            throw new DeleteYourselfNotPossibleException("Cannot delete yourself.");
        }

        var result = await _userRepository.DeleteAsync(cmd.UserId);
        
        if (!result)
        {
            throw new DeleteUserFailException($"Failed to delete user with id: {cmd.UserId}");
        }
    }
}