using SimilaritySearch.Application.Abstractions;
using SimilaritySearch.Application.DTOs;

namespace SimilaritySearch.Application.UseCases.UserUseCases.Handlers;

public class GetAllUsersHandler
{
    private readonly IIdentityService _identityService;

    public GetAllUsersHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    
    public async Task<List<UserDto>> HandleAsync()
    {
        return await _identityService.GetAllAsync();
    }
}