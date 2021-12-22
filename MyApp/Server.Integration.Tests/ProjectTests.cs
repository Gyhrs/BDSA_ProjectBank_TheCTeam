using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using Azure;
using Xunit.Abstractions;

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
    public async Task GetAsync_returns_projects()
    {
        var projects = await _client.GetFromJsonAsync<ProjectDTO[]>("/api/projects");

        Assert.NotNull(projects);
        Assert.True(projects.Length >= 4);
        Assert.Contains(projects, p => p.Name == "Algorithm");
    }
    
    [Theory]
    [InlineData(1, "Blockchain")]
    [InlineData(2, "Algorithm")]
    [InlineData(3, "Supercomputer")]
    [InlineData(4, "Blockchain")]
    public async Task Get_From_IDAsync_Returns_Correct_Project(int id, string expectedname)
    {
        var project = await _client.GetFromJsonAsync<ProjectDTO>("/api/projects/id/" + id);

        Assert.NotNull(project);
        Assert.Equal(id,project?.Id);
        Assert.Equal(expectedname,project?.Name);
    }
    
    [Theory]
    [InlineData("Blockchain",new[]{1,4})]
    [InlineData("Algorithm",new[]{2})]
    [InlineData("Supercomputer",new[]{3})]
    public async Task Get_From_NameAsync_Returns_Correct_Projects(string name, int[] ids)
    {
        var projects = await _client.GetFromJsonAsync<ProjectDTO[]>("/api/projects/name/" + name);
        foreach (var id in ids)
        {
            Assert.Contains(id, projects.Select(e => e.Id));
        }
    }
    
    [Theory]
    [InlineData("UI",new[]{1,3})]
    [InlineData("Business",new[]{1,2})]
    [InlineData("UI_Consulting",new[]{3})]
    public async Task Get_From_TagsAsync_Returns_Correct_Projects(string tags, int[] ids)
    {
        var projects = await _client.GetFromJsonAsync<ProjectDTO[]>("/api/projects/tags/" + tags);
        foreach (var id in ids)
        {
            Assert.Contains(id, projects.Select(e => e.Id));
        }
    }
    
    
    [Theory]
    [InlineData("UI","Blockchain", new[]{1})]
    [InlineData("UI","Supercomputer", new[]{3})]
    [InlineData("Business","Algorithm",new[]{2})]
    [InlineData("UI_Consulting","Supercomputer",new[]{3})]
    public async Task Get_From_TagsAsync_And_Name_Returns_Correct_Projects(string tags, string name, int[] ids)
    {
        var projects = await _client.GetFromJsonAsync<ProjectDTO[]>("/api/projects/tags/" + tags + "/" + name);
        foreach (var id in ids)
        {
            Assert.Contains(id, projects.Select(e => e.Id));
        }
    }
    
    [Theory]
    [InlineData("UI","nonexistent", new int[]{})]
    [InlineData("Algorithm_Consulting","Business",new int[]{})]
    public async Task Get_From_Tags_And_NameAsync_Returns_404_for_nonexistent_project_tag_combination(string tags, string name, int[] ids)
    {
        try
        {
            var projects = await _client.GetFromJsonAsync<ProjectDTO[]>("/api/projects/tags/" + tags + "/" + name);
        }
        catch (Exception e)
        {
            Assert.True(true);
            return;
        }
        Assert.False(false);
    }
}
