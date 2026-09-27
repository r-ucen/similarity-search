using SimilaritySearch.Application.Abstractions;
using SimilaritySearch.Application.Exceptions.User;
using SimilaritySearch.Application.UseCases.UserUseCases.Commands;

namespace SimilaritySearch.Application.UseCases.UserUseCases.Handlers;

public class DeleteUserHandler
{
    private readonly IIdentityService _identityService;
    private readonly IUserContext _userContext;

    public DeleteUserHandler(IIdentityService identityService, IUserContext userContext)
    {
        _identityService = identityService;
        _userContext = userContext;
    }
    
    public async Task Handle(DeleteUserCommand cmd)
    {
        var currentUserId = await _userContext.GetCurrentUserIdAsync();
        if (currentUserId == cmd.UserId)
        {
            throw new DeleteYourselfNotPossibleException("Cannot delete yourself.");
        }

        var result = await _identityService.DeleteAsync(cmd.UserId);
        
        if (!result)
        {
            throw new DeleteUserFailException($"Failed to delete user with id: {cmd.UserId}");
        }
    }
}