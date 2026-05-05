using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimilaritySearch.Infrastructure.Database;
using Testcontainers.Ollama;
using Testcontainers.PostgreSql;

namespace SimilaritySearch.Tests;

public class IntegrationTestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _pgVectorContainer = new PostgreSqlBuilder("pgvector/pgvector:pg16")
        .WithDatabase("test")
        .WithUsername("admin")
        .WithPassword("admin")
        .Build();

    private readonly OllamaContainer _ollamaContainer = new OllamaBuilder("ollama/ollama:0.11.10")
        .Build();

    public async Task InitializeAsync()
    {
        await _pgVectorContainer.StartAsync();
        await _ollamaContainer.StartAsync();
        await _ollamaContainer.ExecAsync(["ollama", "pull", "gemma3:1b"]);
        await _ollamaContainer.ExecAsync(["ollama", "pull", "embeddinggemma:latest"]);

        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        await dbContext.Database.MigrateAsync(); 
    }

    public new async Task DisposeAsync()
    {
        await _pgVectorContainer.DisposeAsync();
        await _ollamaContainer.DisposeAsync();
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ConnectionStrings:DefaultConnection", _pgVectorContainer.GetConnectionString());
        Environment.SetEnvironmentVariable("ollamaUri", _ollamaContainer.GetConnectionString());
        
        builder.ConfigureTestServices(services =>
        {
            services.AddAuthentication(TestAuthHandler.AuthenticationScheme)
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme, null);
        });
    }
}