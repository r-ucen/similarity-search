using SimilaritySearch.Application.Abstractions;
using SimilaritySearch.Application.Abstractions.Repositories;
using SimilaritySearch.Application.Exceptions.User;
using SimilaritySearch.Application.UseCases.UserUseCases.Commands;

namespace SimilaritySearch.Application.UseCases.UserUseCases.Handlers;

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