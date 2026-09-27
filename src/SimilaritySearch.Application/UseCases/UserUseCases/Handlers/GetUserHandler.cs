using SimilaritySearch.Application.Abstractions;
using SimilaritySearch.Application.DTOs;
using SimilaritySearch.Application.UseCases.UserUseCases.Queries;

namespace SimilaritySearch.Application.UseCases.UserUseCases.Handlers;

public class GetUserHandler
{
    private readonly IIdentityService _identityService;

    public GetUserHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    
    public async Task<UserDto> HandleAsync(GetUserQuery query)
    {
        return await _identityService.GetByIdAsync(query.UserId);
    }
}