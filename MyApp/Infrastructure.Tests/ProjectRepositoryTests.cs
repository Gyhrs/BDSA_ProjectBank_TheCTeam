using System;
using Microsoft.EntityFrameworkCore;
using MyApp.Infrastructure;
using MyApp.Shared;
using Xunit;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

public class ProjectRepositoryTests : IDisposable
{
    private readonly IStudyBankContext _context;
    private readonly IProjectRepository _repository;
    private bool disposedValue;

    public ProjectRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<StudyBankContext>();
        builder.UseSqlite(connection);
        var context = new StudyBankContext(builder.Options);
        context.Database.EnsureCreated();

        Supervisor u1 = new Supervisor
        {
            Email = "AntonBertelsen@hotmail.com",
            Name = "Anton",
        };

        Supervisor u2 = new Supervisor
        {
            Email = "LasseGyrs@hotmail.com",
            Name = "Lasse"
        };

        Tag t1 = new Tag
        {
            Name = "UI",
        };

        Tag t2 = new Tag
        {
            Name = "Business",
        };

        Tag t3 = new Tag
        {
            Name = "Consulting",
        };

        Project p1 = new Project
        {
            CreatedBy = u1,
            Description = "This is a amazing project working with talented people.",
            Id = 1,
            Name = "Blockchain",
            StartDate = DateTime.ParseExact("26/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            EndDate = DateTime.ParseExact("28/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            Supervisors = new List<Supervisor> { u1, u2 },
            Tags = new List<Tag> { t1, t2 }
        };

        Project p2 = new Project
        {
            CreatedBy = u1,
            Description = "This is a amazing project working with talented people.",
            Id = 2,
            Name = "Algorithm",
            StartDate = DateTime.ParseExact("26/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            EndDate = DateTime.ParseExact("28/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            Supervisors = new List<Supervisor> { u1, u2 },
            Tags = new List<Tag> { t3 }
        };

        Project p3 = new Project
        {
            CreatedBy = u1,
            Description = "This is a amazing project working with talented people.",
            Id = 3,
            Name = "Supercomputer",
            StartDate = DateTime.ParseExact("26/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            EndDate = DateTime.ParseExact("28/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            Supervisors = new List<Supervisor> { u1, u2 },
            Tags = new List<Tag> { t1, t3 }
        };

        Project p4 = new Project
        {
            CreatedBy = u1,
            Description = "This is a amazing project working with talented people.",
            Id = 4,
            Name = "Blockchain",
            StartDate = DateTime.ParseExact("26/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            EndDate = DateTime.ParseExact("28/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            Supervisors = new List<Supervisor> { u1, u2 },
        };

        context.Projects.AddRange(
            p1, p2, p3, p4
        );

        context.SaveChanges();

        _context = context;
        _repository = new ProjectRepository(_context);
    }

    [Fact]
    public async Task GetAllProjects_Returns_All_Projects()

    {
        // Arrange
        var expected = 4;

        // Act
        var list = await _repository.GetAllProjects();
        var actual = list.Count;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(1, "Blockchain")]
    [InlineData(2, "Algorithm")]
    [InlineData(3, "Supercomputer")]
    public async Task GetProjectFromID_Returns_Correct_Project(int id, string name)
    {
        // Arrange
        var expected = name;

        // Act
        var project = await _repository.GetProjectFromID(id);
        var actual = project.Name;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetProjectFromID_Returns_Null_Given_Nonexisting_ProjectId()
    {
        // Arrange
        // Act
        var actual = await _repository.GetProjectFromID(27);

        // Assert
        Assert.Null(actual);
    }

    [Theory]
    [InlineData("UI", 2)]
    [InlineData("Business", 1)]
    [InlineData("Consulting", 2)]
    [InlineData("Consulting#Business", 3)]
    [InlineData("Consulting#Business#UI", 3)]
    [InlineData("Fast", 0)]

    public async Task GetProjectsFromTags_Returns_Correct_Projects(string tags, int expected)

    {
        // Arrange
        var list = tags.Split("#");

        // Act
        var actual = (await _repository.GetProjectsFromTags(list.ToList())).Count;


        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetProjectsFromTags_Returns_Empty_Given_Nonexisting_Tag()
    {
        // Act
        var actual = await _repository.GetProjectsFromTags(new List<string> { "J#" });

        // Assert
        Assert.Empty(actual);
    }

    [Theory]
    [InlineData(2, "Blockchain")]
    [InlineData(1, "Algorithm")]
    [InlineData(1, "Supercomputer")]
    public async Task GetProjectsFromName_Returns_Correct_Projects(int count, string name)

    {
        // Arrange
        var expected = count;

        // Act
        var actual = (await _repository.GetProjectsFromName(name)).Count;


        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetProjectsFromName_Returns_Empty_Given_Nonexisting_Project_Name()
    {
        // Act
        var actual = await _repository.GetProjectsFromName("Antons projekt");

        // Assert
        Assert.Empty(actual);
    }

    [Fact]
    public async Task GetProjectsFromTagsAndName_Returns_Correct_Projects()
    {
        // Arrange
        var tags = new List<string> {"UI"};
        var title = "Supercomputer";

        var expected = 3;

        // Act
        var projects = await _repository.GetProjectsFromTagsAndName(tags, title);
        var actual = projects.ElementAt(0).Id;

        // Assert
        Assert.Equal(expected, actual);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}