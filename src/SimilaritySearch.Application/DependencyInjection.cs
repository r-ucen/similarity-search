using SimilaritySearch.Application.Abstractions;
using SimilaritySearch.Application.Abstractions.Facades;
using SimilaritySearch.Application.Facade;
using SimilaritySearch.Application.Services;
using SimilaritySearch.Application.UseCases.UserUseCases.Handlers;

namespace SimilaritySearch.Application;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserFacade, UserFacade>();
        services.AddScoped<DeleteUserHandler>();
        services.AddScoped<GetAllUsersHandler>();
        services.AddScoped<GetUserHandler>();
        services.AddScoped<IAdService, AdService>();
        
        return services;
    }
}