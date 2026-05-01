using SimilaritySearch.Application.Abstractions;
using SimilaritySearch.Application.Abstractions.Facades;
using SimilaritySearch.Application.DTOs;
using SimilaritySearch.Application.UseCases.UserUseCases.Commands;
using SimilaritySearch.Application.UseCases.UserUseCases.Handlers;
using SimilaritySearch.Application.UseCases.UserUseCases.Queries;

namespace SimilaritySearch.Application.Facade;

public class UserFacade : IUserFacade
{
    private readonly DeleteUserHandler _deleteUserHandler;
    private readonly GetAllUsersHandler _getAllUsersHandler;
    private readonly GetUserHandler _getUserHandler;
    private readonly  IIdentityService _identityService;
    
    public UserFacade(
        DeleteUserHandler deleteUserHandler,
        GetAllUsersHandler handler,
        GetUserHandler getUserHandler,
        IIdentityService identityService)
    {
        _deleteUserHandler = deleteUserHandler;
        _getAllUsersHandler = handler;
        _getUserHandler = getUserHandler;
        _identityService = identityService;
    }

    public async Task LogOutAsync()
    {
         await _identityService.LogOutAsync();
    }
    
    public async Task DeleteUserAsync(DeleteUserCommand cmd)
    {
        await _deleteUserHandler.Handle(cmd);
    }
    
    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        return await _getAllUsersHandler.HandleAsync();
    }
    
    public async Task<UserDto> GetUserAsync(GetUserQuery query)
    {
        return await _getUserHandler.HandleAsync(query);
    }
}