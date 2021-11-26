using System;
using Microsoft.EntityFrameworkCore;
using MyApp.Infrastructure;
using MyApp.Shared;
using Xunit;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Globalization;

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

        Project p1 = new Project
        {
            CreatedBy = u1,
            Description = "This is a amazing project working with talented people.",
            Id = 1,
            Name = "Blockchain",
            StartDate = DateTime.ParseExact("26/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            EndDate = DateTime.ParseExact("28/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            Supervisors = new List<Supervisor> {u1, u2},
        };

         Project p2 = new Project
        {
            CreatedBy = u1,
            Description = "This is a amazing project working with talented people.",
            Id = 2,
            Name = "Algorithm",
            StartDate = DateTime.ParseExact("26/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            EndDate = DateTime.ParseExact("28/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            Supervisors = new List<Supervisor> {u1, u2},
        };

         Project p3 = new Project
        {
            CreatedBy = u1,
            Description = "This is a amazing project working with talented people.",
            Id = 3,
            Name = "Supercomputer",
            StartDate = DateTime.ParseExact("26/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            EndDate = DateTime.ParseExact("28/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            Supervisors = new List<Supervisor> {u1, u2},
        };

        context.Projects.AddRange(
            p1, p2, p3
        );
        
        context.SaveChanges();

        _context = context;
        _repository = new ProjectRepository(_context);
    }

   

    [Fact]
    public void ReadAsync_Returns_All_Projects()
    {
        // Arrange
        var expected = 3;

        // Act
        var list = _repository.ReadAsync();
        var actual = list.Result.Count;

        // Assert
        Assert.Equal(expected, actual);
    }

     public void Dispose()
    {
        _context.Dispose();
    }
}