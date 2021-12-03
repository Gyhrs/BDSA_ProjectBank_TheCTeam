using System;
using Microsoft.EntityFrameworkCore;
using MyApp.Infrastructure;
using MyApp.Shared;
using Xunit;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

public class UserRepositoryTests : IDisposable
{
    private readonly IStudyBankContext _context;
    private readonly IProjectRepository _repository;
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
            Supervisors = new List<Supervisor> { u1, u2 },
            Students = new List<Student> { u3 }
        };

        context.Projects.AddRange(
            p1, p2, p3, p4
        );

        context.SaveChanges();

        _context = context;
        _repository = new ProjectRepository(_context);
    }

    [Fact]
    public void GetAllUsersAsync_Returns_All_Users()
    {
        // Arrange
        var expected = 5;

        // Act
        var list = _repository.GetAllProjectsAsync();
        var actual = list.Result.Count;

        // Assert
        Assert.Equal(expected, actual);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}