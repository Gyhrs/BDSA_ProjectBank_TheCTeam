//This class has been taken from https://gunnarpeipman.com/aspnet-core-integration-tests-users-roles/

using System.Net.Http.Headers;

namespace MyApp.Server.Integration.Tests;
public static class WebApplicationFactoryExtensions
{
    public static WebApplicationFactory<T> WithAuthentication<T>(this WebApplicationFactory<T> factory, TestClaimsProvider claimsProvider) where T : class
    {
        return factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddScoped<TestClaimsProvider>(_ => claimsProvider);
            });
        });
    }
 
    public static HttpClient CreateClientWithTestAuth<T>(this WebApplicationFactory<T> factory, TestClaimsProvider claimsProvider) where T : class
    {
        var client = factory.WithAuthentication(claimsProvider).CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
 
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
 
        return client;
    }
}