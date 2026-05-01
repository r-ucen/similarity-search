using StockMapSvelte.Application.Abstractions;
using StockMapSvelte.Application.Abstractions.Facades;
using StockMapSvelte.Application.DTOs;
using StockMapSvelte.Application.UseCases.UserUseCases.Commands;
using StockMapSvelte.Application.UseCases.UserUseCases.Handlers;
using StockMapSvelte.Application.UseCases.UserUseCases.Queries;

namespace StockMapSvelte.Application.Facade;

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