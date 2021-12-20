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

    // [Fact]
    // public async Task Post_returns_Created_with_location()
    // {
    //     var character = new CharacterCreateDto
    //     {
    //         GivenName = "Harleen",
    //         Surname = "Quinzel",
    //         AlterEgo = "Harley Quinn",
    //         FirstAppearance = 1992,
    //         Occupation = "Former psychiatrist",
    //         City = "Gotham City",
    //         Gender = Female,
    //         ImageUrl = "https://localhost/images/harley-quinn.png",
    //         Powers = new HashSet<string> { "complete unpredictability", "superhuman agility", "skilled fighter", "intelligence", "emotional manipulation", "immunity to toxins" }
    //     };

    //     var response = await _client.PostAsJsonAsync("/api/characters", character);

    //     Assert.Equal(HttpStatusCode.Created, response.StatusCode);

    //     Assert.Equal(new Uri("http://localhost/api/Characters/5"), response.Headers.Location);

    //     var created = await response.Content.ReadFromJsonAsync<CharacterDetailsDto>();

    //     Assert.NotNull(created);
    //     Assert.Equal(5, created.Id);
    //     Assert.Equal("Harleen", created.GivenName);
    //     Assert.Equal("Quinzel", created.Surname);
    //     Assert.Equal("Harley Quinn", created.AlterEgo);
    //     Assert.Equal("Gotham City", created.City);
    //     Assert.Equal("Former psychiatrist", created.Occupation);
    //     Assert.Equal(1992, created.FirstAppearance);
    //     Assert.Equal(Female, created.Gender);
    //     Assert.Equal("https://localhost/images/harley-quinn.png", created.ImageUrl);
    //     Assert.True(created.Powers.SetEquals(new[] { "complete unpredictability", "superhuman agility", "skilled fighter", "intelligence", "emotional manipulation", "immunity to toxins" }));
    // }
}
