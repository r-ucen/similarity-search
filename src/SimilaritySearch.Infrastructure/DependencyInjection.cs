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
using Microsoft.Extensions.AI;
using OllamaSharp;

namespace SimilaritySearch.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContextFactory<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString, builder => builder.UseVector()));
        
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
        services.AddScoped<IAdRepository, AdRepository>();
        
        services.AddSingleton<YahooQuotes>(new YahooQuotesBuilder().Build());
        
        // Authorization policies
        services.AddAuthorizationBuilder()
            // every authenticated user is a customer
            .AddPolicy("IsCustomer", policy =>
            {
                policy.RequireAuthenticatedUser();
            });
        
        var ollamaUri = configuration.GetSection("ollamaUri").Value ?? throw new InvalidOperationException("Ollama URI is not configured.");
        
        var chatClient = new OllamaApiClient(new Uri(ollamaUri), "deepseek-r1:1.5b");

        var embeddingClient = new OllamaApiClient(new Uri(ollamaUri), "embeddinggemma:latest");
        
        services.AddDistributedMemoryCache();
        
        services.AddChatClient(chatClient)
            .UseDistributedCache();
        
        services.AddEmbeddingGenerator(embeddingClient)
            .UseDistributedCache();
        
        return services;
    }
}