using StockMapSvelte.Application.Abstractions.Facades;
using StockMapSvelte.Application.Facade;
using StockMapSvelte.Application.UseCases.UserUseCases.Handlers;

namespace StockMapSvelte.Application;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserFacade, UserFacade>();
        services.AddScoped<DeleteUserHandler>();
        services.AddScoped<GetAllUsersHandler>();
        services.AddScoped<GetUserHandler>();
        
        return services;
    }
}