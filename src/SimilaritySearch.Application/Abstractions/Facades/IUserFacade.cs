using SimilaritySearch.Application.DTOs;
using SimilaritySearch.Application.UseCases.UserUseCases.Commands;
using SimilaritySearch.Application.UseCases.UserUseCases.Queries;

namespace SimilaritySearch.Application.Abstractions.Facades;

public interface IUserFacade
{
    Task DeleteUserAsync(DeleteUserCommand cmd);
    Task<List<UserDto>> GetAllUsersAsync();
    Task<UserDto> GetUserAsync(GetUserQuery query);
    Task LogOutAsync();
}