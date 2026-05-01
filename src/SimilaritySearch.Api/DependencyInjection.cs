using SimilaritySearch.Api.Services;
using SimilaritySearch.Application.Abstractions;

namespace SimilaritySearch.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddScoped<IUserContext, UserContext>();
        return services;
    }
}