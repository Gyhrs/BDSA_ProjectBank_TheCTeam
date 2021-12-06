using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using MyApp.Server.Controllers;
using MyApp.Shared;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

public class ProjectsControllerTests
{
    [Fact]
    public async Task GetAll_returns_Ok()
    {
        // Arrange
        var logger = new Mock<ILogger<ProjectsController>>();
        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.GetAllProjectsAsync()).ReturnsAsync(Array.Empty<ProjectDTO>());
        var controller = new ProjectsController(logger.Object, repository.Object);


        // Act
        var actual = await controller.GetAll();

        // Assert
        Assert.IsType<OkObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetFromId_Returns_NotFound_Given_Nonexisting_ProjectId()
    {

        // Arrange
        var logger = new Mock<ILogger<ProjectsController>>();
        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.GetProjectFromIDAsync(101)).ReturnsAsync(default(ProjectDTO)); // "For a reference-type it returns null"
        var controller = new ProjectsController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetFromId(101);

        // Assert
        Assert.IsType<NotFoundObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetFromId_Returns_BadRequest_Given_Negative_ProjectId()
    {

        // Arrange
        var logger = new Mock<ILogger<ProjectsController>>();
        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.GetProjectFromIDAsync(-1)).ReturnsAsync(default(ProjectDTO)); // "For a reference-type it returns null"
        var controller = new ProjectsController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetFromId(-1);

        // Assert
        Assert.IsType<BadRequestObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetFromId_Returns_Ok_Given_existing_ProjectId()
    {

        // Arrange
        var logger = new Mock<ILogger<ProjectsController>>();
        var repository = new Mock<IProjectRepository>();

        var project = new ProjectDTO
        (
            1,
            "Blockchain",
            DateTime.ParseExact("26/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            DateTime.ParseExact("28/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            "This is a amazing project working with talented people.",
            new List<string> { "anton@hotmail.com", "nibu@hotmail.com" },
            new List<string> { "andasdton@hotmail.com", "nibabjksdfu@hotmail.com" },
            "createdby@hotmail.com",
            new List<string> { "Business", "Blockchain"}
        );
        repository.Setup(m => m.GetProjectFromIDAsync(1)).ReturnsAsync(project);
        var controller = new ProjectsController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetFromId(1);

        // Assert
        Assert.IsType<OkObjectResult>(actual.Result);
    }

    
    [Fact]
    public async Task GetFromName_Returns_NotFound_Given_Nonexisting_ProjectName()
    {

        // Arrange
        var logger = new Mock<ILogger<ProjectsController>>();
        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.GetProjectsFromNameAsync("Blockchain")).ReturnsAsync(Array.Empty<ProjectDTO>); // "For a reference-type it returns null"
        var controller = new ProjectsController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetFromName("Blockchain");

        // Assert
        Assert.IsType<NotFoundObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetFromName_Returns_BadRequest_Given_Empty_ProjectName()
    {

        // Arrange
        var logger = new Mock<ILogger<ProjectsController>>();
        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.GetProjectsFromNameAsync("Blockchain")).ReturnsAsync(Array.Empty<ProjectDTO>); // "For a reference-type it returns null"
        var controller = new ProjectsController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetFromName("");

        // Assert
        Assert.IsType<BadRequestObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetFromName_Returns_Ok_Given_Existing_ProjectName()
    {

        // Arrange
        var logger = new Mock<ILogger<ProjectsController>>();
        var repository = new Mock<IProjectRepository>();

        var project = new ProjectDTO
        (
            1,
            "Blockchain",
            DateTime.ParseExact("26/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            DateTime.ParseExact("28/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            "This is a amazing project working with talented people.",
            new List<string> { "anton@hotmail.com", "nibu@hotmail.com" },
            new List<string> { "andasdton@hotmail.com", "nibabjksdfu@hotmail.com" },
            "createdby@hotmail.com",
            new List<string> { "Business", "Blockchain"}
        );
        repository.Setup(m => m.GetProjectsFromNameAsync("Blockchain")).ReturnsAsync(new List<ProjectDTO> { project });
        var controller = new ProjectsController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetFromName("Blockchain");

        // Assert
        Assert.IsType<OkObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetFromTags_Returns_Ok_Given_Existing_Tag()
    {

        // Arrange
        var logger = new Mock<ILogger<ProjectsController>>();
        var repository = new Mock<IProjectRepository>();

        var projects = new List<ProjectDTO>() {
            new ProjectDTO
            (
                1,
                "Blockchain",
                DateTime.ParseExact("26/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                DateTime.ParseExact("28/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                "This is a amazing project working with talented people.",
                new List<string> { "anton@hotmail.com", "nibu@hotmail.com" },
                new List<string> { "andasdton@hotmail.com", "nibabjksdfu@hotmail.com" },
                "createdby@hotmail.com",
                new List<string> {"Blockchain"}
            ),
            new ProjectDTO
            (
                2,
                "Metaverse",
                DateTime.ParseExact("26/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                DateTime.ParseExact("28/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                "This is a amazing project working with talented people.",
                new List<string> { "lasse@hotmail.com", "peter@hotmail.com" },
                new List<string> { "hej@med.com", "dig@hotmail.com" },
                "createdby@hotmail.com",
                new List<string> { "Business"}
            )
        };
        repository.Setup(m => m.GetProjectsFromTagsAsync(new List<string> { "Business", "Blockchain"} )).ReturnsAsync(projects);
        var controller = new ProjectsController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetFromTags("Business#Blockchain");

        // Assert
        Assert.IsType<OkObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetFromTags_Returns_BadRequest_Given_Empty_Tag_String()
    {

        // Arrange
        var logger = new Mock<ILogger<ProjectsController>>();
        var repository = new Mock<IProjectRepository>();

        repository.Setup(m => m.GetProjectsFromTagsAsync(new List<string> { "" } )).ReturnsAsync(Array.Empty<ProjectDTO>);
        var controller = new ProjectsController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetFromTags("");

        // Assert
        Assert.IsType<BadRequestObjectResult>(actual.Result);
    }

    [Fact]
    public async Task GetFromTags_Returns_NotFound_Given_Nonexistent_Tag()
    {

        // Arrange
        var logger = new Mock<ILogger<ProjectsController>>();
        var repository = new Mock<IProjectRepository>();

        repository.Setup(m => m.GetProjectsFromTagsAsync(new List<string> { "Testing" })).ReturnsAsync(Array.Empty<ProjectDTO>);
        var controller = new ProjectsController(logger.Object, repository.Object);

        // Act
        var actual = await controller.GetFromTags("Testing");

        // Assert
        Assert.IsType<NotFoundObjectResult>(actual.Result);
    }
}