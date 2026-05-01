using StockMapSvelte.Api.Services;
using StockMapSvelte.Application.Abstractions;

namespace StockMapSvelte.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddScoped<IUserContext, UserContext>();
        return services;
    }
}