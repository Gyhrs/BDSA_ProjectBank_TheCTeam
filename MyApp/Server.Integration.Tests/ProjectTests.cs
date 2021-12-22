namespace MyApp.Server.Integration.Tests;

public class ProjectTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    private TestClaimsProvider _provider;

    private readonly HttpClient _client;

    public ProjectTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _provider = TestClaimsProvider.WithUserClaims();
        _client = factory.CreateClientWithTestAuth(_provider);
    }

    [Fact]
    public async Task Get_returns_projects()
    {
        var projects = await _client.GetFromJsonAsync<ProjectDTO[]>("/api/projects");

        Assert.NotNull(projects);
        Assert.True(projects.Length >= 4);
        Assert.Contains(projects, p => p.Name == "Algorithm");
    }
}
