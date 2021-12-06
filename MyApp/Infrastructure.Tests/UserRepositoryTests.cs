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

public class UserRepositoryTests : IDisposable
{
    private readonly IStudyBankContext _context;
    private readonly IUserRepository _repository;
    private bool disposedValue;

    public UserRepositoryTests()
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

        Student u3 = new Student
        {
            Email = "Klartmanner@hotmail.com",
            Name = "Klart"
        };

        Student u4 = new Student
        {
            Email = "CronvalPhilip@hotmail.com",
            Name = "Cronval"
        };

        Student u5 = new Student
        {
            Email = "FastMartin@hotmail.com",
            Name = "Martin"
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
            Supervisors = new List<Supervisor> { u2 },
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
            Supervisors = new List<Supervisor> { u1 },
            Tags = new List<Tag> { t1, t3 },
            Students = new List<Student> { u4, u5 }

        };

        Project p4 = new Project
        {
            CreatedBy = u1,
            Description = "This is a amazing project working with talented people.",
            Id = 4,
            Name = "Blockchain",
            StartDate = DateTime.ParseExact("26/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            EndDate = DateTime.ParseExact("28/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            Supervisors = new List<Supervisor> { u2 },
            Students = new List<Student> { u3 }
        };

        context.Projects.AddRange(
            p1, p2, p3, p4
        );

        context.SaveChanges();

        _context = context;
        _repository = new UserRepository(_context);
    }

    [Fact]
    public async Task GetAllUsersAsync_Returns_All_Users()
    {
        // Arrange
        var expectedCount = 5;

        // Act
        var users = await _repository.GetAllUsersAsync();
        var actualCount = users.Count;

        // Assert
        Assert.Equal(expectedCount, actualCount);

        Assert.Collection(users,
            user => Assert.Equal("AntonBertelsen@hotmail.com", user.Email),
            user => Assert.Equal("LasseGyrs@hotmail.com", user.Email),
            user => Assert.Equal("CronvalPhilip@hotmail.com", user.Email), 
            user => Assert.Equal("FastMartin@hotmail.com", user.Email),
            user => Assert.Equal("Klartmanner@hotmail.com", user.Email)
        );
    }

    [Theory]
    [InlineData("AntonBertelsen@hotmail.com", "Anton")]
    [InlineData("LasseGyrs@hotmail.com", "Lasse")]
    [InlineData("CronvalPhilip@hotmail.com", "Cronval")]
    [InlineData("FastMartin@hotmail.com", "Martin")]
    [InlineData("Klartmanner@hotmail.com", "Klart")]
    public async Task GetUserFromEmailAsync_Returns_Correct_User(string email, string name)
    {
        // Arrange
        var expected = name;

        // Act
        var user = await _repository.GetUserFromEmailAsync(email);
        var actual = user.Name;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(3, "CronvalPhilip@hotmail.com#FastMartin@hotmail.com", 2)]
    [InlineData(4, "Klartmanner@hotmail.com", 1)]
    public async Task GetStudentsFromProjectIdAsync_Returns_Correct_Students(int projectId, string names, int count)
    {
        // Arrange
        var expected = names.Split("#");

        // Act
        var users = await _repository.GetStudentsFromProjectIDAsync(projectId);

        Assert.Equal(count, users.Count);

        // Assert
        foreach (var name in expected)
        {
            Assert.Contains(name, users.Select(u => u.Email));
        }
    }

    [Theory]
    [InlineData(1, "AntonBertelsen@hotmail.com#LasseGyrs@hotmail.com", 2)]
    [InlineData(2, "LasseGyrs@hotmail.com", 1)]
    [InlineData(3, "AntonBertelsen@hotmail.com", 1)]
    [InlineData(4, "LasseGyrs@hotmail.com", 1)]
    public async Task GetSupervisorsFromProjectIdAsync_Returns_Correct_Supervisors(int projectId, string names, int count)
    {
        // Arrange
        var expected = names.Split("#");

        // Act
        var users = await _repository.GetSupervisorsFromProjectIDAsync(projectId);

        Assert.Equal(count, users.Count);   
        // Assert
        foreach (var name in expected)
        {
            Assert.Contains(name, users.Select(u => u.Email));
        }
    }


    [Fact]
    public async Task GetSupervisorsFromProjectIdAsync_Returns_Empty_Given_Non_Existent_Project_Id()
    {
        // Act
        var users = await _repository.GetSupervisorsFromProjectIDAsync(5);

        Assert.Equal(0,users.Count);
    }

    [Fact]
    public async Task GetStudentsFromProjectIdAsync_Returns_Empty_Given_Non_Existent_Project_Id()
    {
        // Act
        var users = await _repository.GetStudentsFromProjectIDAsync(5);

        Assert.Equal(0,users.Count);
    }


    [Fact]
    public async Task GetUserFromEmailAsync_Returns_Null_When_Given_Nonexisting_Email() {
        // Act
        var actual = await _repository.GetUserFromEmailAsync("MickeyMouse@Disney.com");

        // Assert
        Assert.Null(actual);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}