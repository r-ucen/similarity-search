using SimilaritySearch.Application.Abstractions;
using SimilaritySearch.Application.Abstractions.Repositories;
using SimilaritySearch.Application.DTOs;
using SimilaritySearch.Application.UseCases.UserUseCases.Queries;

namespace SimilaritySearch.Application.UseCases.UserUseCases.Handlers;

public class GetUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;

    public GetUserHandler(IUserRepository userRepository, IUserContext userContext)
    {
        _userRepository = userRepository;
        _userContext = userContext;
    }
    
    public async Task<UserDto> HandleAsync(GetUserQuery query)
    {
        return await _userRepository.GetByIdAsync(query.UserId);
    }
}