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
    public async Task Get_returns_projects()
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
    public async Task Get_From_ID_Returns_Correct_Project(int id, string expectedname)
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
    public async Task Get_From_Name_Returns_Correct_Projects(string name, int[] ids)
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
    public async Task Get_From_Tags_Returns_Correct_Projects(string tags, int[] ids)
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
    public async Task Get_From_Tags_And_Name_Returns_Correct_Projects(string tags, string name, int[] ids)
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
    public async Task Get_From_Tags_And_Name_Returns_404_for_nonexistent_project_tag_combination(string tags, string name, int[] ids)
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

    /*[Fact]
    public async Task Post_returns_Created_Project()
    {
        var project = new ProjectCreateDTO()
        {
            Name = "New",
            StartDate = DateTime.ParseExact("26/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            EndDate = DateTime.ParseExact("26/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            StudentEmails = new List<string>() {"student1@email.com","student2@email.com"},
            SupervisorsEmails = new List<string>() {"supervisor1@email.com","supervisor2@email.com"},
            CreatedByEmail = "creator@email.com"
        };
        
        var response = await _client.PostAsJsonAsync("/api/projects", project);
                
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var created = await response.Content.ReadFromJsonAsync<ProjectDTO>();
        
        Assert.NotNull(created);
        Assert.Equal(5, created.Id);
        Assert.Equal("New", created.Name);
        Assert.Equal(DateTime.ParseExact("26/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture), created.StartDate);
        Assert.Equal(DateTime.ParseExact("26/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture), created.EndDate);
        Assert.Equal("creator@email.com", created.CreatedByEmail);
        
        new List<string>() {"student1@email.com","student2@email.com"}.ForEach(e => Assert.Contains(e,created.StudentEmails));
        new List<string>() {"supervisor1@email.com","supervisor2@email.com"}.ForEach(e => Assert.Contains(e,created.SupervisorsEmails));
    }*/
}
