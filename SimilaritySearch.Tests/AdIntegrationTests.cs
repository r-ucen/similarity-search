using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;

namespace SimilaritySearch.Tests;

[CollectionDefinition("IntegrationTests")]
public class SharedTestCollection : ICollectionFixture<IntegrationTestWebApplicationFactory> { }

[Collection("IntegrationTests")]
public class AdIntegrationTests
{
    private readonly IntegrationTestWebApplicationFactory _factory;
    private readonly HttpClient _client;
    
    public AdIntegrationTests(IntegrationTestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
    }
}