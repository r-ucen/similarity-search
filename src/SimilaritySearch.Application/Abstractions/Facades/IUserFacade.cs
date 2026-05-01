using StockMapSvelte.Application.DTOs;
using StockMapSvelte.Application.UseCases.UserUseCases.Commands;
using StockMapSvelte.Application.UseCases.UserUseCases.Queries;

namespace StockMapSvelte.Application.Abstractions.Facades;

public interface IUserFacade
{
    Task DeleteUserAsync(DeleteUserCommand cmd);
    Task<List<UserDto>> GetAllUsersAsync();
    Task<UserDto> GetUserAsync(GetUserQuery query);
    Task LogOutAsync();
}