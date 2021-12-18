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

        Student s1 = new Student
        {
            Email = "nibu@itu.dk",
            Name = "Nikoline Burman",
            Program = "SWU",
        };

        Student s2 = new Student
        {
            Email = "lakl@itu.dk",
            Name = "Lasse Klausen",
            Program = "SWU",
        };

        Student s3 = new Student
        {
            Email = "tugy@itu.dk",
            Name = "Tue Gyhrs",
            Program = "SWU",
        };

        Supervisor s4 = new Supervisor
        {
            Email = "phcr@itu.dk",
            Name = "Philip Cronval"
        };

        Supervisor s5 = new Supervisor
        {
            Email = "palo@itu.dk",
            Name = "Paolo"
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

        context.Users.AddRange(
            s1, s2, s3, s4, s5
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
    [InlineData("UI#Business", 1)]
    [InlineData("Consulting#Business#UI", 0)]
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
        var tags = new List<string> { "UI" };
        var title = "Supercomputer";

        var expected = 3;

        // Act
        var projects = await _repository.GetProjectsFromTagsAndName(tags, title);
        var actual = projects.ElementAt(0).Id;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task CreateProject_Adds_Project_To_DB()
    {
        // Arrange
        var inputProject = new ProjectCreateDTO()
        {
            CreatedBy = "Anton",
            CreatedByEmail = "AntonBertelsen@hotmail.com",
            Description = "A project made by Lars",
            EndDate = DateTime.ParseExact("28/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            StartDate = DateTime.ParseExact("23/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            Name = "Lars Project",
            StudentEmails = new List<string>() {"nibu@itu.dk", "lakl@itu.dk", "tugy@itu.dk"},
            SupervisorEmails = new List<string>() {"phcr@itu.dk", "palo@itu.dk"},
            Tags = new List<string>() {"UI", "Business"}
        };

        // Act
        var actualMethod = await _repository.CreateProject(inputProject);

        var actualDB = _context.Projects.Where(p => p.CreatedBy.Name == "Anton" && p.Name == "Lars Project").FirstOrDefault();

        // Assert
        Assert.Equal(inputProject.CreatedBy, actualDB.CreatedBy.Name);
        Assert.Equal(inputProject.CreatedByEmail, actualDB.CreatedBy.Email);
        Assert.Equal(inputProject.Name, actualDB.Name);
        Assert.Equal(inputProject.Description, actualDB.Description);
        Assert.Equal(inputProject.EndDate, actualDB.EndDate);
        Assert.Equal(inputProject.StartDate, actualDB.StartDate);
        Assert.Equal(inputProject.StudentEmails, actualDB.Students.Select(s => s.Email).ToList());
        Assert.Equal(inputProject.SupervisorEmails, actualDB.Supervisors.Select(s => s.Email).ToList());
        Assert.Equal(inputProject.Tags, actualDB.Tags.Select(s => s.Name).ToList());
        Assert.Equal(5, actualDB.Id);
    }

    [Fact]
    public async Task CreateProject_Adds_Project_To_DB_Returns_Correct_DTO()
    {
        // Arrange
        var inputProject = new ProjectCreateDTO()
        {
            CreatedBy = "Anton",
            CreatedByEmail = "AntonBertelsen@hotmail.com",
            Description = "A project made by Lars",
            EndDate = DateTime.ParseExact("28/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            StartDate = DateTime.ParseExact("23/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            Name = "Lars Project",
            StudentEmails = new List<string>() {"nibu@itu.dk", "lakl@itu.dk", "tugy@itu.dk"},
            SupervisorEmails = new List<string>() {"phcr@itu.dk", "palo@itu.dk"},
            Tags = new List<string>() {"Business", "UI"}
        };
        // Act
        var actualMethod = await _repository.CreateProject(inputProject);

        var actualDB = _context.Projects.Where(p => p.CreatedBy.Name == "Anton" && p.Name == "Lars Project").FirstOrDefault();

        // Assert
        Assert.Equal(inputProject.CreatedBy, actualMethod.CreatedBy);
        Assert.Equal(inputProject.CreatedByEmail, actualMethod.CreatedByEmail);
        Assert.Equal(inputProject.Name, actualMethod.Name);
        Assert.Equal(inputProject.Description, actualMethod.Description);
        Assert.Equal(inputProject.EndDate, actualMethod.EndDate);
        Assert.Equal(inputProject.StartDate, actualMethod.StartDate);
        Assert.Equal(inputProject.StudentEmails, actualMethod.StudentEmails);
        Assert.Equal(inputProject.SupervisorEmails, actualMethod.SupervisorsEmails);
        Assert.Equal(inputProject.Tags, actualMethod.Tags);
        Assert.Equal(5, actualMethod.Id);
    }
    [Fact]
    public async Task UpdateProject_Updates_All_Values_In_Correct_Project()
    {
        // Arrange
        var updateProject = new ProjectUpdateDTO()
        {
            Id  = 1,
            CreatedBy = "Anton",
            CreatedByEmail = "AntonBertelsen@hotmail.com",
            Description = "A project made by Lars",
            EndDate = DateTime.ParseExact("28/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            StartDate = DateTime.ParseExact("23/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            Name = "Lars Project",
            StudentEmails = new List<string>() {"nibu@itu.dk", "lakl@itu.dk", "tugy@itu.dk"},
            SupervisorEmails = new List<string>() {"phcr@itu.dk", "palo@itu.dk"},
            Tags = new List<string>() {"Business", "UI"}
        };
        // Act
        var expected = await _repository.UpdateProject(updateProject);
        var actual = _context.Projects.Where(p => p.Id == 1).FirstOrDefault();

        // Assert
        Assert.Equal(updateProject.Name, actual.Name);
        Assert.Equal(updateProject.CreatedBy, actual.CreatedBy.Name);
        Assert.Equal(updateProject.CreatedByEmail, actual.CreatedBy.Email);
        Assert.Equal(updateProject.Description, actual.Description);
        Assert.Equal(updateProject.StartDate, actual.StartDate);
        Assert.Equal(updateProject.EndDate, actual.EndDate);
        Assert.Equal(updateProject.StudentEmails, actual.Students.Select(s => s.Email).ToList());
        Assert.Equal(updateProject.SupervisorEmails, actual.Supervisors.Select(s => s.Email).ToList());
        Assert.Equal(updateProject.Tags, actual.Tags.Select(t => t.Name).ToList());
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}