using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Resend;
using SimilaritySearch.Application.Abstractions;
using SimilaritySearch.Application.Abstractions.Repositories;
using SimilaritySearch.Infrastructure.Database;
using SimilaritySearch.Infrastructure.Identity;
using SimilaritySearch.Infrastructure.Repositories;
using SimilaritySearch.Infrastructure.Services;
using YahooQuotesApi;

namespace SimilaritySearch.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContextFactory<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));
        
        services.AddScoped(p => 
            p.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());
        
        services.AddIdentityApiEndpoints<ApplicationUser>(options => {
                options.SignIn.RequireConfirmedAccount = true;
            })
            .AddRoles<Role>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        
        // Resend
        services.AddOptions<ResendClientOptions>()
            .Bind(configuration.GetSection("Resend"));
        services.AddHttpClient<IResend, ResendClient>();
        services.AddTransient<IEmailSender<ApplicationUser>, ResendEmailSender>();
        
        // DI
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IUserRepository, UserRepository>();
        
        services.AddSingleton<YahooQuotes>(new YahooQuotesBuilder().Build());
        
        // Authorization policies
        services.AddAuthorizationBuilder()
            // every authenticated user is a customer
            .AddPolicy("IsCustomer", policy =>
            {
                policy.RequireAuthenticatedUser();
            });
        
        return services;
    }
}