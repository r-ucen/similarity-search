using StockMapSvelte.Application.Abstractions.Repositories;
using StockMapSvelte.Application.DTOs;

namespace StockMapSvelte.Application.UseCases.UserUseCases.Handlers;

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