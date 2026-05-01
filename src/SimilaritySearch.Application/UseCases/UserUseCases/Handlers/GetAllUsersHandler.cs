using SimilaritySearch.Application.Abstractions.Repositories;
using SimilaritySearch.Application.DTOs;

namespace SimilaritySearch.Application.UseCases.UserUseCases.Handlers;

public class GetAllUsersHandler
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<List<UserDto>> HandleAsync()
    {
        return await _userRepository.GetAllAsync();
    }
}